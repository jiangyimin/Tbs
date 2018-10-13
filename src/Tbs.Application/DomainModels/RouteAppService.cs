using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Tbs.DomainModels.Dto;
using System.Reflection;
using Tbs.Configuration;
using Tbs.DomainServices;
using Tbs.Timing;
using Abp.Linq;

namespace Tbs.DomainModels
{
    public class RouteAppService : TbsAppServiceBase, IRouteAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<RouteIdentify> _routeIdentifyRepository;
        private readonly IRepository<RouteWorker> _workerRepository;
        private readonly IRepository<RouteTask> _taskRepository;
        private readonly IRepository<PreRoute> _preRouteRepository;
        private readonly IRepository<PreRouteTask> _preRouteTaskRepository;
        private readonly IRepository<Vehicle> _preWorkerRepository;

        private readonly IComboAppService _comboAppservice;

        private readonly SigninDomainService _signinService;
        private readonly IRepository<TaskType> _taskTypeRepository;

        private readonly IRepository<ArticleRecord> _recordRepository;
        private readonly IRepository<DaySettle> _settleRepository;

        public RouteAppService(IRepository<Route> routeRepository, IRepository<RouteIdentify> routeIdentifyRepository, 
                    IRepository<RouteWorker> workerRepository, IRepository<RouteTask> taskRepository, 
                    IRepository<PreRoute> preRouteRepository, IRepository<PreRouteTask> preRouteTaskRepository, 
                    IRepository<Vehicle> preWorkerRepository, IComboAppService comboAppService, 
                    SigninDomainService signinService, IRepository<TaskType> taskTypeRepository, 
                    IRepository<ArticleRecord> recordRepository, IRepository<DaySettle> settleRepository)
        {
            _routeRepository = routeRepository;
            _routeIdentifyRepository = routeIdentifyRepository;
            _workerRepository = workerRepository;
            _taskRepository = taskRepository;
            _preRouteRepository = preRouteRepository;
            _preRouteTaskRepository = preRouteTaskRepository;
            _preWorkerRepository = preWorkerRepository;

            _comboAppservice = comboAppService;
            _signinService = signinService;
            _taskTypeRepository = taskTypeRepository;
            _recordRepository = recordRepository;
            _settleRepository = settleRepository;
        }

        public List<RouteDto> GetRoutes(int depotId, DateTime carryoutDate, string sorting)
        {
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            var o = _routeRepository.GetAll().Where($"DepotId={depotId} AND CarryoutDate=\"{dateString}\"").OrderBy(sorting).ToList();
            return new List<RouteDto>(o.Select(ObjectMapper.Map<RouteDto>));
        }

        public List<RouteIdentifyDto> GetRouteIdentifies(int routeId, string sorting)
        {
            var o = _routeIdentifyRepository.GetAll().Where(e => e.RouteId == routeId).OrderBy(sorting).ToList();

            return new List<RouteIdentifyDto>(o.Select(ObjectMapper.Map<RouteIdentifyDto>).ToList());            
        }

        public List<RouteDto> GetRoutesActive(int depotId, DateTime carryoutDate, string sorting)
        {
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            var o = _routeRepository.GetAll().Where($"DepotId={depotId} AND CarryoutDate=\"{dateString}\"").OrderBy(sorting).ToList();
            return new List<RouteDto>(o.Select(ObjectMapper.Map<RouteDto>).Where(r => r.Status != "安排"));
        }

        public async Task<RouteDto> Insert(RouteDto input)
        {
                var entity = ObjectMapper.Map<Route>(input);
                await _routeRepository.InsertAsync(entity);
                CurrentUnitOfWork.SaveChanges();
                return ObjectMapper.Map<RouteDto>(entity);
        }

        public async Task<RouteDto> Update(RouteDto input)
        {
            var entity = _routeRepository.Get(input.Id);
            ObjectMapper.Map<RouteDto, Route>(input, entity);
            await _routeRepository.UpdateAsync(entity);
            return ObjectMapper.Map<RouteDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _routeRepository.DeleteAsync(id);
        }

        #region Operate
        public async Task<int> CreateFromPre(int depotId, DateTime carryoutDate)
        {
            var list = _preRouteRepository.GetAllList(e=>e.DepotId == depotId);
            var routeRoles = GetRouteRolesAsync();
            foreach (PreRoute a in list)
            {
                Route route = new Route();
                route.DepotId = depotId;
                route.CarryoutDate = carryoutDate;
                route.Status = "安排";
                route.RouteTypeId = a.RouteTypeId;
                route.RouteCn = a.RouteCn;
                route.RouteName = a.RouteName;
                route.SetoutTime = a.SetoutTime;
                route.ReturnTime = a.ReturnTime;
                route.VehicleId = a.VehicleId;
                route.Remark = a.Remark;

                route.Tasks = new List<RouteTask>();
                var preTasks = _preRouteTaskRepository.GetAllList(e => e.PreRouteId == a.Id);
                foreach (PreRouteTask t in preTasks)
                {
                    RouteTask task = new RouteTask();
                    task.ArriveTime = t.ArriveTime;
                    task.OutletId = t.OutletId;
                    task.TaskTypeId = t.TaskTypeId;
                    route.Tasks.Add(task);
                }

                if (a.VehicleId.HasValue) 
                {
                    route.Workers = new List<RouteWorker>();
                    var preWorker = await _preWorkerRepository.FirstOrDefaultAsync(e=> e.Id == a.VehicleId);
                    if (preWorker != null)
                    {
                        for (int i = 0; i < routeRoles.Count; i++)
                        {
                            RouteRole role = routeRoles[i];
                            RouteWorker worker = new RouteWorker();
                            PropertyInfo prop = typeof(Vehicle).GetProperty($"Worker{i+1}Id");
                            int? id = (int?)prop.GetValue(preWorker);
                            if (id.HasValue && SetWorkerInfo(depotId, id.Value, worker))
                            {
                                worker.RouteRoleId = role.Id;
                                worker.RouteRoleName = role.Name;
                                route.Workers.Add(worker);
                            }
                        }
                    }
                }
                await _routeRepository.InsertAsync(route);
            }

            return list.Count;
        }
        public async Task<int> CreateFrom(int depotId, DateTime carryoutDate, DateTime fromDate)
        {
            var ids = GetTaskTypeListCreatFrom();
            var list = _routeRepository.GetAllList(e=>e.DepotId == depotId && e.CarryoutDate == fromDate);
            foreach (Route a in list)
            {
                Route route = new Route();
                route.DepotId = depotId;
                route.CarryoutDate = carryoutDate;
                route.Status = "安排";
                route.RouteTypeId = a.RouteTypeId;
                route.RouteCn = a.RouteCn;
                route.RouteName = a.RouteName;
                route.SetoutTime = a.SetoutTime;
                route.ReturnTime = a.ReturnTime;
                route.VehicleId = a.VehicleId;
                route.Remark = a.Remark;

                route.Tasks = new List<RouteTask>();
                var fromTasks = _taskRepository.GetAllList(e => e.RouteId == a.Id);
                foreach (RouteTask t in fromTasks)
                {
                    if (!ids.Contains(t.TaskTypeId)) continue;          // skip not in ids

                    RouteTask task = new RouteTask();
                    task.ArriveTime = t.ArriveTime;
                    task.OutletId = t.OutletId;
                    task.TaskTypeId = t.TaskTypeId;
                    route.Tasks.Add(task);
                }

                var workers =  _workerRepository.GetAllList(e => e.RouteId == a.Id);
                foreach (RouteWorker w in workers)
                {
                    RouteWorker worker = new RouteWorker();
                    worker.WorkerId = w.WorkerId;
                    worker.WorkerCn = w.WorkerCn;
                    worker.WorkerName = w.WorkerName;
                    worker.RouteRoleId = w.RouteRoleId;
                    worker.RouteRoleName = w.RouteRoleName;
                    if (route.Workers == null)
                        route.Workers = new List<RouteWorker>();
                    route.Workers.Add(worker);
                }
                    
                await _routeRepository.InsertAsync(route);
            }

            return list.Count;
        }

        public List<RouteDto> CheckActivate(int depotId, DateTime carryoutDate)
        {
            var o = _routeRepository.GetAllList(e => e.DepotId==depotId && e.CarryoutDate==carryoutDate && e.Status == "安排");
            foreach (Route a in o)
            {
                CheckActivateEnabled(a, false);
            }
            return new List<RouteDto>(o.Select(ObjectMapper.Map<RouteDto>));
        }

        public async Task<int> Activate(int depotId, DateTime carryoutDate, string status)
        {
            var o = _routeRepository.GetAllList(e => e.DepotId==depotId && e.CarryoutDate==carryoutDate && e.Status == "安排");
            int count = 0;
            foreach (Route a in o)
            {
               CheckActivateEnabled(a, true);
               if (a.ActivateInfo == string.Empty)
               {
                    a.Status = status;
                    await _routeRepository.UpdateAsync(a);
                    count++;
               }
            }
            return count;
        }

        public async Task<int> ActivateSelects(List<int> ids, string status)
        {
            int count = 0;
            foreach (int id in ids)
            {
                var route = _routeRepository.Get(id);
                CheckActivateEnabled(route, true);
               if (route.ActivateInfo == string.Empty && route.Status == "安排")
               {
                    route.Status = status;
                    await _routeRepository.UpdateAsync(route);
                    count++;
               }               
            }
            return count;
        }

        public RouteWorkerMatchResult FindWorkersForArticle(int depotId, DateTime carryoutDate, Worker worker, string style)
        {
            var index = 0;
            Route route = SearchRoute(depotId, carryoutDate, worker, style, out index);
            if (route == null)
                return new RouteWorkerMatchResult("无线路或此时段不能领还物品");
           
            RouteWorker rw = route.Workers[index];

            // filter scan cards in lend
            if (style == "1" && rw.LendTime.HasValue && DateTime.Now.Subtract(rw.LendTime.Value) < TimeSpan.FromSeconds(30))
            {
                return new RouteWorkerMatchResult("刚领物品，请过30秒后再领!");
            }

            RouteType type = DomainManager.GetRouteType(route.RouteTypeId);
            RouteRole role = DomainManager.GetRouteRole(rw.RouteRoleId);
            RouteRole roleSG = GetAnotherRoleInSameGroup(type, role);

            var ret = new RouteWorkerMatchResult(route.Id);
            ret.RouteCn = route.RouteCn;
            if (route.VehicleId.HasValue) {
                var vehicle = DomainManager.GetVehicle(depotId, route.VehicleId.Value);
                if (vehicle != null) ret.VehicleCn = vehicle.Cn;
            }
                
            WorkerMatched wm = new WorkerMatched(rw.Id, rw.RecordList, worker.Id, worker.Cn, worker.Name, worker.IDCardNo, worker.Photo, role.Id, role.Name);
            ret.AddWorker(SetArticlesLended(depotId, wm));
            if (roleSG != null) {
                var rwSG = _workerRepository.FirstOrDefault(w => w.RouteId == route.Id && w.RouteRoleId == roleSG.Id);
                if (rwSG != null) {
                    var wSG = DomainManager.GetWorkerById(depotId, rwSG.WorkerId);
                    wm = new WorkerMatched(rwSG.Id, rwSG.RecordList, wSG.Id, wSG.Cn, wSG.Name, wSG.IDCardNo, wSG.Photo, roleSG.Id, roleSG.Name);
                    ret.AddWorker(SetArticlesLended(depotId, wm));
                }
            }
            return ret;
        }

        public Route FindRouteForIdentify(int depotId, int workerId, out int subworkerId)
        {
            var routes = _routeRepository.GetAllIncluding(r => r.Workers)
                    .Where(r => r.DepotId == depotId && r.CarryoutDate == DateTime.Today && (r.Status != "安排" && r.Status != "还物")).OrderByDescending(r => r.SetoutTime).ToList();
            
            foreach (Route route in routes)
            {
                if (route.Status != "领物" && TimeUtil.DistToNow(DateTime.Today, route.SetoutTime) < 0)   // 未到出发时间
                    continue;

                var type = DomainManager.GetRouteType(route.RouteTypeId);
                var mainRole = DomainManager.GetRouteRoles(type.Id).FirstOrDefault(r => r.Name == type.IdentifyRoleName);
                if (mainRole == null)
                    continue;

                var routeWorker = route.Workers.FirstOrDefault(rw => rw.RouteRoleName == mainRole.Name && rw.WorkerId == workerId);
                if (routeWorker == null)  
                    continue;

                var subRole = DomainManager.GetRouteRoles(type.Id).FirstOrDefault(r => r.Name != mainRole.Name && r.PeerGroupNo == mainRole.PeerGroupNo);
                if (subRole == null) {
                    subworkerId = -1;
                }
                else {
                    var subRouteWorker = route.Workers.FirstOrDefault(rw => rw.RouteRoleId == subRole.Id);
                    subworkerId = subRouteWorker != null ? subRouteWorker.WorkerId : 0;
                }
                
                route.Tasks = _taskRepository.GetAllList(t => t.RouteId == route.Id);
                return route;
            }

            subworkerId = -2;
            return null;
        }

        public void SetIdentifyTime(int taskId, int routeId, int outletId)
        {
            Logger.Debug($"设置身份确认时间（{taskId}）（{routeId})({outletId}）");
            if (taskId > 0) // then must have routeId
            {
                var task = _taskRepository.Get(taskId);
                task.IdentifyTime = DateTime.Now;
                _taskRepository.Update(task);
            }
            else
            {
                if (routeId >= 0 && outletId > 0)
                {
                    var entity = new RouteIdentify() {
                        RouteId = routeId,
                        OutletId = outletId, 
                        IdentifyTime = DateTime.Now
                    };
                    _routeIdentifyRepository.Insert(entity);
                }
            }
        }

        #endregion 
        

        #region Worker and Task
        public List<RouteWorkerDto> GetRouteWorkers(int id, string sorting)
        {
            var o = _workerRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting).ToList();

            return new List<RouteWorkerDto>(o.Select(ObjectMapper.Map<RouteWorkerDto>).ToList());

        }
        public async Task<RouteWorkerDto> UpdateSon(RouteWorkerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<RouteWorkerDto, RouteWorker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteWorkerDto>(entity);
        }

        public async Task<RouteWorkerDto> InsertSon(RouteWorkerDto input)
        {
            var entity = ObjectMapper.Map<RouteWorker>(input);

            await _workerRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteWorkerDto>(entity);
        }

        public async Task DeleteSon(int id)
        {
            await _workerRepository.DeleteAsync(id);
        }

        public List<RouteTaskDto> GetRouteTasks(int id, string sorting)
        {
            var o = _taskRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting).ToList();

            return new List<RouteTaskDto>(o.Select(ObjectMapper.Map<RouteTaskDto>).ToList());

        }
        public async Task<RouteTaskDto> UpdateSon2(RouteTaskDto input)
        {
            var entity = await _taskRepository.GetAsync(input.Id);
            ObjectMapper.Map<RouteTaskDto, RouteTask>(input, entity);

            await _taskRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteTaskDto>(entity);
        }

        public async Task<RouteTaskDto> InsertSon2(RouteTaskDto input)
        {
            var entity = ObjectMapper.Map<RouteTask>(input);

            await _taskRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteTaskDto>(entity);
        }

        public async Task DeleteSon2(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }


        public string DaySettle(int depotId, DateTime carryoutDate, int settleId) 
        {
            var query = _routeRepository.GetAll().Where(r => r.DepotId == depotId && r.CarryoutDate == carryoutDate);
            int countTotal = query.Count(); 
            int countWarn = query.Where(r => r.Status != "还物").Count();

            string settleResult = $"今日安排了{countTotal}条线路";
            if (countWarn > 0) 
                settleResult += $"({countWarn}条线路未还物)";
            else
                settleResult += ", 已全部完成";

            if (settleId > 0)
            {
                var s = _settleRepository.Get(settleId);
                s.RoutesCount = countTotal;
                s.Message = $"【{settleResult}】";               
                _settleRepository.Update(s);
                return settleResult;
            }
            else 
            {
                return countWarn == 0 ? countTotal.ToString() : settleResult;
            }
        }

        public async Task<List<RouteWorkerStatDto>> Stat(int depotId, DateTime begin, DateTime end)
        {
            if (end.Subtract(begin) > TimeSpan.FromDays(90))
                throw new Abp.UI.UserFriendlyException("日期不能超过90天!");
                
            var query = _routeRepository.GetAll().Where(a => a.DepotId == depotId && 
                a.CarryoutDate >= begin && a.CarryoutDate <= end);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            Dictionary<string, CommonStat> dict = new Dictionary<string, CommonStat>();
            List<RouteRole> roles = DomainManager.GetRouteRoles();          
            foreach (var r in entities)
            {
                foreach (var w in _workerRepository.GetAllList(w=>w.RouteId == r.Id))
                {
                    string worker = w.WorkerCn + " " + w.WorkerName;
                    if (!dict.ContainsKey(worker)) 
                        dict[worker] = new CommonStat(6);
                
                    CommonStat cs = dict[worker];          
                    int i = GetIndexByName(w.RouteRoleName, roles); 
                    if (i < 0 || i > 5) continue;

                    cs.DtHS.Add(r.CarryoutDate);
                    cs.Qtums[i] += 1;                    
                }
            }

            var dtos = new List<RouteWorkerStatDto>();
            foreach (var item in dict.ToList().OrderBy(a => a.Key))
            {
                RouteWorkerStatDto dto = new RouteWorkerStatDto(item.Key);
                CommonStat cs = item.Value;
                dto.DayCount = cs.DtHS.Count();
                for (int i = 0; i < 6; i++)
                {
                    PropertyInfo propCount = typeof(RouteWorkerStatDto).GetProperty($"RoleCount{i+1}");
                    propCount.SetValue(dto, cs.Qtums[i]);                 
                }

                dtos.Add(dto);
            }
            return dtos;
        }

        #endregion

        #region util

        private List<int> GetTaskTypeListCreatFrom()
        {
            var  taskTypes = _taskTypeRepository.GetAllList();
            List<int> taskTypeIds = new List<int>();
            string setting = SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.TaskTypeListForRoutesFrom, AbpSession.TenantId.Value).Result;
            List<string> cns = new List<string>(setting.Split('|'));
            foreach (TaskType type in taskTypes)
            {
                if (cns.Contains(type.Cn))
                    taskTypeIds.Add(type.Id);
            }
            return taskTypeIds;
        }
        private void CheckActivateEnabled(Route route, bool action)
        {

            // check routeType
            var routeType = DomainManager.GetRouteType(route.RouteTypeId);

            int dist = Tbs.Timing.TimeUtil.DistToNow(route.CarryoutDate, route.SetoutTime);
            if (dist < 0 && Math.Abs(dist) > routeType.ActivateAhead)
            {
                route.ActivateInfo = $"未到激活提前量({routeType.ActivateAhead})";
                return;
            }

            var workers = _workerRepository.GetAllList(w => w.RouteId == route.Id);
            // check routeRoles
            var roles = DomainManager.GetRouteRoles(route.RouteTypeId);
            foreach (RouteRole role in roles) 
            {
                if (role.Required) {
                    if (workers.FirstOrDefault( w => w.RouteRoleId == role.Id) == null) {
                        route.ActivateInfo += $"{role.Name} ";
                        // return;
                    }                   
                }
            }
            if (!string.IsNullOrEmpty(route.ActivateInfo)) {
                route.ActivateInfo += "没有安排";
                return;
            }

            // check signin
            var mustSignin = SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.MustSigninOnRouteActive, AbpSession.TenantId.Value).Result;
            string names = string.Empty;
            foreach (RouteWorker worker in workers)
            {
                Worker w = DomainManager.GetWorkerById(route.DepotId, worker.WorkerId);
                if (w == null) 
                    continue;
                names += !_signinService.IsSignin(w) ? w.Name + " " : string.Empty;
                if (names != string.Empty)
                {
                    if (mustSignin == "true" || action == false) {
                        route.ActivateInfo = names + " ";
                    }
                }
            }
            if (!string.IsNullOrEmpty(route.ActivateInfo))
                route.ActivateInfo += "未签到";
            else
                route.ActivateInfo = string.Empty;
        }

        private List<RouteRole> GetRouteRolesAsync()
        {
            var types = _comboAppservice.GetComboItems("RouteType", "Id", "Name").Result;
            int typeId = int.Parse(types[0].Value);
            return DomainManager.GetRouteRoles(typeId);
        }

        private bool SetWorkerInfo(int depotId, int workerId, RouteWorker worker)
        {
            var workers = DomainManager.GetWorkers(depotId);
            Worker w = workers.SingleOrDefault(e=>e.Id == workerId);
            if (w == null) return false;
            worker.WorkerId = w.Id;
            worker.WorkerCn = w.Cn;
            worker.WorkerName = w.Name;
            return true;
        }

        private Route SearchRoute(int depotId, DateTime carryoutDate, Worker worker, string style, out int index)
        {
            List<Route> routes = new List<Route>();
            if (style == "1")
                routes = _routeRepository.GetAllIncluding(r => r.Workers)
                    .Where(r => r.DepotId == depotId && r.CarryoutDate == carryoutDate && r.Status != "安排")
                    .OrderBy(r => r.SetoutTime).ToList();
            else
                routes = _routeRepository.GetAllIncluding(r => r.Workers)
                    .Where(r => r.DepotId == depotId && r.CarryoutDate == carryoutDate && (r.Status == "领物" || r.Status == "还物"))
                    .OrderBy(r => r.SetoutTime).ToList();

            foreach (Route route in routes)
            {
                RouteType type = DomainManager.GetRouteType(route.RouteTypeId);

                if (style == "1")       // 领用
                {
                    if (TimeUtil.DistToNow(DateTime.Today, route.ReturnTime) > 0) continue;  // 已超过线路的回来时间
                    int dist = TimeUtil.DistToNow(DateTime.Today, route.SetoutTime);
                    if (dist < 0 && dist < -type.ArticleAhead) continue;  //"现在未到允许领物时间";
                    if (dist > type.ArticleDeadline) continue; // "已过最后的领物时间";
                }
                // else                    // 归还
                // {
                //     if (TimeUtil.DistToNow(DateTime.Today, route.ReturnTime) > 120) 
                //         continue;
                // }

                for (int i = 0; i < route.Workers.Count; i++)
                {
                    RouteWorker rw = route.Workers[i];
                    if (!DomainManager.GetRouteRole(rw.RouteRoleId).NeedArticle) continue;
                    if (rw.ReturnTime.HasValue) continue;

                    if (route.Workers[i].WorkerId == worker.Id)
                    {
                        index = i;
                        return route;
                    }
                }
            }

            index = -1;
            return null;
        }

        private RouteRole GetAnotherRoleInSameGroup(RouteType type, RouteRole role)
        {
            if (string.IsNullOrEmpty(role.PeerGroupNo)) return null;

            foreach(RouteRole r in DomainManager.GetRouteRoles(type.Id))
            {
                if (r.Id != role.Id && r.PeerGroupNo == role.PeerGroupNo)
                    return r;
            }
            return null;
        }

        private WorkerMatched SetArticlesLended(int depotId, WorkerMatched wm)
        {
            if (!string.IsNullOrEmpty(wm.RecordList))
            {
                foreach (string item in wm.RecordList.Split())
                {
                    int recordId = 0;
                    if (int.TryParse(item, out recordId))
                    {
                        try
                        {
                            var r = _recordRepository.Get(recordId);
                            Article a = DomainManager.GetArticle(depotId, r.ArticleId);
                            ArticleType type = DomainManager.GetArticleType(a.ArticleTypeId);
                            wm.ArticlesLended.Add(new ArticleLended(a.Id, a.Cn, a.Name, type.Name, a.Rfid));
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn($"生成还物时出错(RouteWorkerId:{wm.Id})" + ex.Message);
                        }
                    }
                }
            }
            return wm;
        }

        private int GetIndexByName(string name, List<RouteRole> roles)
        {
            for (int i = 0; i < roles.Count; i++)
                if (name == roles[i].Name) 
                    return i;
            return -1;
        }

        #endregion
    }
}