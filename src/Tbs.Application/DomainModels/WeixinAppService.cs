using System;
using System.Linq;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainServices;
using Tbs;
using Tbs.DomainModels.Dto;
using Tbs.Configuration;
using Tbs.Authorization.Users;
using System.Text.RegularExpressions;

namespace Tbs.DomainModels
{
    /* THIS IS JUST A SAMPLE. */
    public class WeixinAppService : TbsAppServiceBase, IWeixinAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Worker> _workerRepository;
        private readonly IRepository<Manager> _managerRepository;
        private readonly SigninDomainService _signinService;
        private readonly IRouteAppService _routeAppService;

        public WeixinAppService(IRepository<User, long> userRepository, IRepository<Worker> workerRepository, IRepository<Manager> managerRepository, 
                SigninDomainService signinService, IRouteAppService routeAppService)
        {
            _userRepository = userRepository;
            _workerRepository = workerRepository;
            _managerRepository = managerRepository;
            _signinService = signinService;
            _routeAppService = routeAppService;
        }

        public WxIdentifyDto Login(int tenantId, string workerCn, string password, string deviceId)
        {
            WxIdentifyDto dto = new WxIdentifyDto(tenantId, workerCn);
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            using (AbpSession.Use(tenantId, null))
            {
                var worker = DomainManager.GetWorkerByCn(workerCn);
                if (worker == null || password != worker.Password) {
                    dto.ErrorMessage = "用户名或密码密码";
                    return dto;
                }

                if (string.IsNullOrWhiteSpace(worker.DeviceId))
                {
                    worker.DeviceId = deviceId;
                    _workerRepository.Update(worker);
                }
                else {
                    if (worker.DeviceId != deviceId) {
                        dto.ErrorMessage = "微信设备ID与登记不符";
                        return dto;                       
                    }                   
                }

                dto.DepotId = worker.DepotId;
                dto.WorkerName = worker.Name;
                dto.IDNumber = worker.IDNumber;
                dto.Photo = worker.Photo != null ? Convert.ToBase64String(worker.Photo) : null;

                // judge right routeRole and subworker
                dto.UseRoute = DomainManager.GetDepotById(worker.DepotId).UseRouteForIdentify;
                int subWorkerId = GetSubWorkerAndSetVehicle(worker, dto);
                if (dto.RouteId == -1)
                {
                    dto.ErrorMessage = "登录工号未安排合适的线路";
                    return dto;
                }
                var mustHave = SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.IdentifyNeedSubWorker, AbpSession.TenantId.Value).Result;
                if (mustHave == "true" && subWorkerId <= 0)
                {
                    dto.ErrorMessage = "设定必须要辅助交接员";
                    return dto;                                           
                }

                // get and set subWorker to dto
                Worker subWorker = DomainManager.GetWorkerById(worker.DepotId, subWorkerId);
                if (subWorker != null) 
                {
                    dto.WorkerCn2 = subWorker.Cn;
                    dto.WorkerName2 = subWorker.Name;
                    dto.IDNumber2 = subWorker.IDNumber;
                    dto.Photo2 = subWorker.Photo != null ? Convert.ToBase64String(subWorker.Photo) : null;
                }
            }
            //Logger.Debug("身份确认" + dto.UseRoute + dto.ToString());
            return dto;
        }


        public string Signin(int tenantId, string workerCn, double lon, double lat, double accuracy)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            using (AbpSession.Use(tenantId, null))
            {
                var worker = DomainManager.GetWorkerByCn(workerCn);
                if (worker == null)
                    return "系统无对应的工号";

                var depot = DomainManager.GetDepotById(worker.DepotId);
                if (!depot.Latitude.HasValue || !depot.Longitude.HasValue)
                    return "中心的经纬度不能为空";

                double dis = Geo.GetDistance(depot.Latitude.Value, depot.Longitude.Value, lat, lon);
                if (dis > 400 + accuracy)
                    return "请到中心后再签到";

                return _signinService.Signin(worker);
            }
        }

        public void SetIdentifyTime(int taskId, int routeId, int outletId)
        {
             _routeAppService.SetIdentifyTime(taskId, routeId, outletId);
         }

        public void ResetDeviceId(int id)
        {
            Worker worker = _workerRepository.Get(id);
            worker.DeviceId = null;
            _workerRepository.Update(worker);
        }
        public void ResetAllDeviceId()
        {
            foreach (Worker worker in _workerRepository.GetAll())
            {
                worker.DeviceId = null;
                _workerRepository.Update(worker);
            }
        }

        public string[] ProcessTextMessage(int tenantId, string fromUser, string content)
        {
            using (AbpSession.Use(tenantId, null))
            {
            string toUser = null; 
            string replyMessage = null;
            int hours = 0;
            int commandCode = ParseCommand(content, out hours);

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                string ret = string.Empty;

                switch (commandCode) 
                {
                    case 0:
                    case 1:
                        User user = _userRepository.FirstOrDefault(u => u.UserName == fromUser);
                        if (user != null && user.RoleName == "Dispatcher" 
                            && !string.IsNullOrEmpty(user.WhName) && Regex.IsMatch(user.WhName, @"^\d*$")) 
                        {
                            toUser = user.WhName;
                            replyMessage = $"{user.UserName} {user.Name}向您发送了申请({content})，可回复同意或不同意";

                            var upper = _managerRepository.FirstOrDefault(m => m.Cn == user.WhName);
                            if (upper != null) {
                                if (commandCode == 0)  // 申请密码用缺省。
                                    hours = int.Parse(SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.DefautPasswordExpiryHours, AbpSession.TenantId.Value).Result);
                                upper.Mobile = $"{fromUser} {hours}";
                                _managerRepository.Update(upper);
                            }
                            ret = $"您发送了请假申请，系统已转发到您的上级({toUser})";
                        }
                        else {
                            ret = "您目前不需要通过微信请假";
                        };
                        break;
                    case 2:
                        var manager = _managerRepository.FirstOrDefault(m => m.Cn == fromUser);
                        if (manager == null) {
                            ret = "您无权限发同意命令";
                            break;
                        }
                    
                        if (string.IsNullOrEmpty(manager.Mobile))
                        {
                            ret = "目前没有申请，您无需发同意命令";
                            break;
                        }

                        string[] parts = manager.Mobile.Split();
                        user = _userRepository.FirstOrDefault(u => u.UserName == parts[0]);
                        if (user != null) 
                        {
                            if (parts.Length > 1) 
                                int.TryParse(parts[1], out hours);
                            if (hours == 0) 
                                hours = int.Parse(SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.DefautPasswordExpiryHours, AbpSession.TenantId.Value).Result);

                            user.SetOpPassword(user.GenOpPassword(), hours);
                            _userRepository.Update(user);
                            manager.Mobile = null;
                            _managerRepository.Update(manager);

                            toUser = parts[0];
                            replyMessage = $"{manager.Name}已同意您的申请。操作密码为{user.GetOpPassword()}";

                            ret = $"您已同意{user.UserName} {user.Name}的申请!";
                        }
                        else {
                            ret = "找不到申请者的账号，请与系统管理员联系";
                        }
                        break;

                    case 3:
                        manager = _managerRepository.FirstOrDefault(m => m.Cn == fromUser);
                        if (manager == null) {
                            ret = "您无权限发同意命令";
                            break;
                        }               
                        if (string.IsNullOrEmpty(manager.Mobile)) {
                            ret = "目前没有申请，您无需发送不同意命令";
                            break;
                        }
                    
                        toUser = manager.Mobile.Split()[0];
                        replyMessage = $"{manager.Name} 不同意你的申请！请电话联系";
                        manager.Mobile = null;
                        _managerRepository.Update(manager);
                    
                        ret = "系统接受到你的不同意命令";
                        break;
                    case 4:
                        user = _userRepository.FirstOrDefault(u => u.UserName == fromUser);
                        if (user != null && user.RoleName == "Dispatcher" 
                            && !string.IsNullOrEmpty(user.WhName) && Regex.IsMatch(user.WhName, @"^\d*$")) 
                        {
                            toUser = user.WhName;
                            replyMessage = $"{user.UserName} {user.Name}向您发送了销假通知。";
                            ret = $"系统已转发您的销假信息到您的领导";
                        }
                        else {
                            ret = "您目前不需要通过微信销假";
                        };
                        break;
                        
                    default:
                        ret = "业务系统不处理此消息";
                        break;
                }
                return new string[] {ret, toUser, replyMessage };
            } 
            }                      
        }
       
        #region util

        private int ParseCommand(string command, out int hours)
        {
            int commandCode = -1;
            string[] parts = command.Split();
            if (parts[0] == "密码")
                commandCode = 0;
            else if (parts[0] == "请假")
                commandCode = 1;
            else if (parts[0] == "同意")
                commandCode = 2;
            else if (parts[0] == "不同意")
                commandCode = 3;
            else if (parts[0] == "销假")
                commandCode = 4;

            // get num
            int num = 0;
            for (int i = 1; i < parts.Length; i++) {
                string result = Regex.Replace(parts[i], @"[^0-9]+", "");
                if (int.TryParse(result, out num)) 
                {
                    break;
                }
            }
            if (num == 0)
                hours = int.Parse(SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.DefautPasswordExpiryHours, AbpSession.TenantId.Value).Result);
            else
                hours = num * 24;

            return commandCode;
        }

        private int GetSubWorkerAndSetVehicle(Worker mainWorker, WxIdentifyDto dto)
        {
            int subWorkerId = 0;
            if (dto.UseRoute == false)
            {
                var vehicles = DomainManager.GetVehicles(mainWorker.DepotId);
                foreach (Vehicle vehicle in vehicles)
                {
                    if (vehicle.MainWorkerId.HasValue && vehicle.MainWorkerId.Value == mainWorker.Id)
                    {
                        dto.VehicleCn = vehicle.Cn;
                        dto.License = vehicle.License;
                        dto.VehiclePhoto = vehicle.Photo != null ? Convert.ToBase64String(vehicle.Photo) : null;

                        subWorkerId = vehicle.SubWorkerId.HasValue ? vehicle.SubWorkerId.Value : 0;
                        break;
                    }
                }
            }
            else        // Get SubWorker and Vechile From Route 
            {
                Route r = _routeAppService.FindRouteForIdentify(mainWorker.DepotId, mainWorker.Id, out subWorkerId);
                if (r != null) 
                {
                    dto.RouteId = r.Id;
                    if (r.Tasks != null) { 
                        foreach (RouteTask task in r.Tasks) {
                            Outlet outlet = DomainManager.GetOutlets(mainWorker.DepotId).FirstOrDefault(o => o.Id == task.OutletId);
                            string tm = task.IdentifyTime.HasValue ? task.IdentifyTime.Value.ToString("HH:mm:ss") : "未交接";
                            var type = DomainManager.GetTaskType(task.TaskTypeId); 
                            dto.Tasks.Add(new WeixinTaskDto(
                                task.Id, task.ArriveTime, type==null?task.TaskTypeId.ToString():type.Name, outlet.Cn, outlet.Name, tm, task.Remark));
                        }
                    }
                    if (r.VehicleId.HasValue) {
                        var vehicle = DomainManager.GetVehicle(r.DepotId, r.VehicleId.Value);
                        dto.VehicleCn = vehicle.Cn;
                        dto.License = vehicle.License;
                        dto.VehiclePhoto = Convert.ToBase64String(vehicle.Photo);                   
                    }
                }
                else {
                    dto.RouteId = -1;
                }
            }

            return subWorkerId;
        }
        #endregion
    }
}