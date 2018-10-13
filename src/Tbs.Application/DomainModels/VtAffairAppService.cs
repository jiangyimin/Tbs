using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Tbs.DomainModels.Dto;
using Tbs.Timing;
using Tbs.Configuration;
using Tbs.DomainServices;
using Abp.Linq;
using System.Reflection;

namespace Tbs.DomainModels
{
    public class VtAffairAppService : TbsAppServiceBase, IVtAffairAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<VtAffair> _affairRepository;
        private readonly IRepository<VtAffairWorker> _workerRepository;

        private readonly IRepository<DaySettle> _settleRepository;
        private readonly SigninDomainService _signinService;


        public VtAffairAppService(IRepository<VtAffair> affairRepository, IRepository<VtAffairWorker> workerRepository, 
                IRepository<DaySettle> settleRepository, SigninDomainService signinService)
        {
            _affairRepository = affairRepository;
            _workerRepository = workerRepository;
            _settleRepository = settleRepository;
            _signinService = signinService;
        }


        public List<VtAffairDto> GetAffairs(int depotId, DateTime carryoutDate, string sorting)
        {
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            var o = _affairRepository.GetAll().Where($"DepotId={depotId} AND CarryoutDate=\"{dateString}\"").OrderBy(sorting).ToList();
            return new List<VtAffairDto>(o.Select(ObjectMapper.Map<VtAffairDto>));

        }

        public List<VtAffairDto> GetAffairsActive(int depotId, DateTime carryoutDate, string sorting)
        {
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            var o = _affairRepository.GetAll().Where($"DepotId={depotId} AND CarryoutDate=\"{dateString}\"").OrderBy(sorting).ToList();
            return new List<VtAffairDto>(o.Select(ObjectMapper.Map<VtAffairDto>).Where(e=>e.Status != "安排"));
        }

        public async Task<VtAffairDto> Insert(VtAffairDto input)
        {
                var entity = ObjectMapper.Map<VtAffair>(input);
                await _affairRepository.InsertAsync(entity);
                CurrentUnitOfWork.SaveChanges();
                return ObjectMapper.Map<VtAffairDto>(entity);
        }

        public async Task<VtAffairDto> Update(VtAffairDto input)
        {
            var entity = _affairRepository.Get(input.Id);
            ObjectMapper.Map<VtAffairDto, VtAffair>(input, entity);
            await _affairRepository.UpdateAsync(entity);
            return ObjectMapper.Map<VtAffairDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _affairRepository.DeleteAsync(id);
        }

        public async Task<int> CreateFrom(int depotId, DateTime carryoutDate, DateTime fromDate)
        {
            var list = _affairRepository.GetAllList(e=>e.DepotId == depotId && e.CarryoutDate == fromDate);
            foreach (VtAffair a in list)
            {
                VtAffair affair = new VtAffair();
                affair.DepotId = depotId;
                affair.CarryoutDate = carryoutDate;
                affair.Status = "安排";
                affair.VaultTypeId = a.VaultTypeId;
                affair.VtName = a.VtName;
                affair.StartTime = a.StartTime;
                affair.EndTime = a.EndTime;
                affair.Remark = a.Remark;

                var workers =  _workerRepository.GetAllList(e => e.VtAffairId == a.Id);
                foreach (VtAffairWorker w in workers)
                {
                    VtAffairWorker worker = new VtAffairWorker();
                    worker.WorkerId = w.WorkerId;
                    worker.WorkerCn = w.WorkerCn;
                    worker.WorkerName = w.WorkerName;
                    worker.VaultRoleId = w.VaultRoleId;
                    worker.VaultRoleName = w.VaultRoleName;
                    if (affair.Workers == null)
                        affair.Workers = new List<VtAffairWorker>();
                    affair.Workers.Add(worker);
                }
                    
                await _affairRepository.InsertAsync(affair);
            }

            return list.Count;
        }

        public List<VtAffairDto> CheckActivate(int depotId, DateTime carryoutDate)
        {
            var o = _affairRepository.GetAllList(e => e.DepotId==depotId && e.CarryoutDate==carryoutDate && e.Status == "安排");
            foreach (VtAffair a in o)
            {
                CheckActivateEnabled(a, false);
            }
            return new List<VtAffairDto>(o.Select(ObjectMapper.Map<VtAffairDto>));
        }
        public async Task<int> Activate(int depotId, DateTime carryoutDate, string status)
        {
            var o = _affairRepository.GetAllList(e => e.DepotId==depotId && e.CarryoutDate==carryoutDate && e.Status == "安排");
            int count = 0;
            foreach (VtAffair a in o)
            {
                CheckActivateEnabled(a, true);
                if (a.ActivateInfo == string.Empty) {
                    a.Status = status;
                    await _affairRepository.UpdateAsync(a);
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
                var affair = _affairRepository.Get(id);
                CheckActivateEnabled(affair, true);
                if (affair.ActivateInfo == string.Empty && affair.Status == "安排")
                {
                    affair.Status = status;
                    await _affairRepository.UpdateAsync(affair);
                    count++;
                }               
            }
            return count;
        }

        public string GetCheckStatus(int affairId)
        {
            var workers = _workerRepository.GetAllList(e=>e.VtAffairId == affairId);
            int numCheckin = 0;
            int numCheckout = 0;
            foreach (VtAffairWorker vtWorker in workers)
            {
                // count num checkin and checkout
                if (vtWorker.CheckIn != null) numCheckin += 1;
                if (vtWorker.CheckOut != null) numCheckout += 1;
            }

            VtAffair affair = _affairRepository.Get(affairId);
            VaultType vaultType = DomainManager.GetVaultType(affair.VaultTypeId);
            int dist = TimeUtil.DistToNow(DateTime.Today, affair.StartTime);
            if (numCheckin == workers.Count && numCheckout < workers.Count && dist < 8 * 60)
                return "-";
            else 
                return "";
        }
        public KeyValuePair<string, string> CheckWorker(Worker worker, int affairId)
        {
            VtAffair affair = _affairRepository.Get(affairId);
            VaultType vaultType = DomainManager.GetVaultType(affair.VaultTypeId);

            // if (TimeUtil.DistToNow(DateTime.Today, affair.EndTime) > 0 )
            //     return new KeyValuePair<string, string>("error", "现在已过了结束时间");
                
            int dist = TimeUtil.DistToNow(DateTime.Today, affair.StartTime);
            if (dist < 0 && dist < -vaultType.VerifyAhead)
                return new KeyValuePair<string, string>("error", "现在未到允许进库时间");

            var workers = _workerRepository.GetAllList(e=>e.VtAffairId == affairId);
            int numCheckin = 0;
            int numCheckout = 0;
            KeyValuePair<string, string> ret;
            foreach (VtAffairWorker vtWorker in workers)
            {
                // count num checkin and checkout
                if (vtWorker.CheckIn != null) numCheckin += 1;
                if (vtWorker.CheckOut != null) numCheckout += 1;

                if (vtWorker.WorkerId == worker.Id)
                {
                    if (vtWorker.CheckIn == null && dist > vaultType.VerifyDeadline)
                        ret = new KeyValuePair<string, string>("error", "已过最后的进库时间");
                    
                    if (vtWorker.CheckIn == null) {
                        vtWorker.CheckIn = DateTime.Now;
                        _workerRepository.Update(vtWorker);
                        ret = new KeyValuePair<string, string>("ok", "任务正常签入");                       
                    }
                    else
                    {
                        if (vtWorker.CheckOut == null)
                        {
                            if (dist > vaultType.VerifyDeadline || vtWorker.CheckIn.HasValue) 
                            {
                                vtWorker.CheckOut = DateTime.Now;
                                _workerRepository.Update(vtWorker);
                                ret = new KeyValuePair<string, string>("ok", "任务正常签出");    
                            }  
                            else
                            {
                                ret = new KeyValuePair<string, string>("error", "未到签出时间");    
                            }
                        }
                        else
                        {
                            ret = new KeyValuePair<string, string>("error", "已签出过");      
                        }                 
                    }
                }
            }

            if (default(KeyValuePair<string, string>).Equals(ret)) {
                return new KeyValuePair<string, string>("error", "此任务未安排此人");
            }
            
            // process checkIn or Checkout
            string msg = ret.Value;
            if (msg.Contains("签入") && workers.Count == numCheckin + 1)
            {
                ret = new KeyValuePair<string, string>("ok", "-" + msg);
            }
            if (msg.Contains("签出") && workers.Count == numCheckout + 1)
            {
                ret = new KeyValuePair<string, string>("ok", "+" + msg);
            }
            return ret;
        }

        public string DaySettle(int depotId, DateTime carryoutDate, int settleId) 
        {
            int countTotal = 0; 
            int countWarn = 0;

            string checkResult = null;
            foreach (var a in _affairRepository.GetAllList(a => a.DepotId == depotId && a.CarryoutDate == carryoutDate))
            {
                foreach (var w in _workerRepository.GetAllList(w => w.VtAffairId == a.Id).ToList())
                {
                    countTotal++;
                    if (!w.CheckIn.HasValue || !w.CheckOut.HasValue) {
                        checkResult += w.WorkerName + " ";
                        countWarn++;
                    }
                }
            }

            string settleResult = $"金库安排{countTotal}人上岗";
            if (countWarn > 0) settleResult += $"({countWarn}人异常)";

            if (settleId > 0)
            {
                var s = _settleRepository.Get(settleId);
                s.VtAffairsCount = countTotal;
                s.Message += $"【{settleResult}】";                
                _settleRepository.Update(s);
                return settleResult;
            }
            else
            {
                return checkResult == null ? countTotal.ToString() : checkResult + "未正常签入/签出";
            }
        }


        #region Task
        public List<VtAffairWorkerDto> GetAffairWorkers(int id, string sorting)
        {
            var o = _workerRepository.GetAll().Where(e => e.VtAffairId == id).OrderBy(sorting).ToList();

            return new List<VtAffairWorkerDto>(o.Select(ObjectMapper.Map<VtAffairWorkerDto>).ToList());

        }
        public async Task<VtAffairWorkerDto> UpdateSon(VtAffairWorkerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<VtAffairWorkerDto, VtAffairWorker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<VtAffairWorkerDto>(entity);
        }

        public async Task<VtAffairWorkerDto> InsertSon(VtAffairWorkerDto input)
        {
            var entity = ObjectMapper.Map<VtAffairWorker>(input);

            await _workerRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<VtAffairWorkerDto>(entity);
        }

        public async Task DeleteSon(int id)
        {
            await _workerRepository.DeleteAsync(id);
        }

        public async Task<List<AffairWorkerStatDto>> Stat(int depotId, DateTime begin, DateTime end)
        {
            if (end.Subtract(begin) > TimeSpan.FromDays(90))
                throw new Abp.UI.UserFriendlyException("日期不能超过90天!");
                
            var query = _affairRepository.GetAll().Where(a => a.DepotId == depotId && 
                a.CarryoutDate >= begin && a.CarryoutDate <= end);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            Dictionary<string, CommonStat> dict = new Dictionary<string, CommonStat>();
            List<VaultRole> roles = DomainManager.GetVaultRoles();          
            foreach (var r in entities)
            {
                foreach (var w in _workerRepository.GetAllList(w=>w.VtAffairId == r.Id))
                {
                    string worker = w.WorkerCn + " " + w.WorkerName;
                    if (!dict.ContainsKey(worker)) 
                        dict[worker] = new CommonStat(6);
                
                    CommonStat cs = dict[worker];

                    if (!w.CheckIn.HasValue)
                        cs.Qtums[0] += 1;
                    if (!w.CheckOut.HasValue)
                        cs.Qtums[1] += 1;
                           
                    int i = GetIndexByName(w.VaultRoleName, roles); 
                    if (i < 0 || i > 3) continue;

                    cs.DtHS.Add(r.CarryoutDate);
                    cs.Qtums[i + 2] += 1;                    
                }
            }

            var dtos = new List<AffairWorkerStatDto>();
            foreach (var item in dict.ToList().OrderBy(a => a.Key))
            {
                AffairWorkerStatDto dto = new AffairWorkerStatDto(item.Key);
                CommonStat cs = item.Value;
                dto.DayCount = cs.DtHS.Count();
                dto.NoCheckInCount = cs.Qtums[0];
                dto.NoCheckOutCount = cs.Qtums[1];
                for (int i = 0; i < 4; i++)
                {
                    PropertyInfo propCount = typeof(AffairWorkerStatDto).GetProperty($"RoleCount{i+1}");
                    propCount.SetValue(dto, cs.Qtums[i+2]);                 
                }

                dtos.Add(dto);
            }
            return dtos;
        }

        #endregion

        #region util
        private void CheckActivateEnabled(VtAffair affair, bool action)
        {
            // check routeType
            var vaultType = DomainManager.GetVaultType(affair.VaultTypeId);

            int dist = Tbs.Timing.TimeUtil.DistToNow(affair.CarryoutDate, affair.StartTime);
            if (dist < 0 && Math.Abs(dist) > vaultType.ActivateAhead)
            {
                affair.ActivateInfo = $"未到激活提前量({vaultType.ActivateAhead})";
                return;
            }

            var workers = _workerRepository.GetAllList(w => w.VtAffairId == affair.Id);
            // check routeRoles
            var roles = DomainManager.GetVaultRoles(affair.VaultTypeId);
            foreach (VaultRole role in roles) 
            {
                if (role.Required) {
                    if (workers.FirstOrDefault( w => w.VaultRoleId == role.Id) == null) {
                        affair.ActivateInfo += $"{role.Name} ";
                    }                   
                }
            }
            
            if (!string.IsNullOrEmpty(affair.ActivateInfo)) {
                affair.ActivateInfo += "没有安排";
                return;
            }

            // check signin
            var mustSignin = SettingManager.GetSettingValueForTenantAsync(SettingNames.WorkFlow.MustSigninOnRouteActive, AbpSession.TenantId.Value).Result;
            string names = string.Empty;
            foreach (VtAffairWorker worker in workers)
            {
                Worker w = DomainManager.GetWorkerById(affair.DepotId, worker.WorkerId);
                names += !_signinService.IsSignin(w) ? w.Name + " " : string.Empty;
                if (names != string.Empty)
                {
                    if (mustSignin == "true" || action == false) {
                        affair.ActivateInfo = names + " ";
                    }
                }
            }
            if (!string.IsNullOrEmpty(affair.ActivateInfo))
                affair.ActivateInfo += "未签到";
            else
                affair.ActivateInfo = string.Empty;
        }

        private int GetIndexByName(string name, List<VaultRole> roles)
        {
            for (int i = 0; i < roles.Count; i++)
                if (name == roles[i].Name) 
                    return i;
            return -1;
        }

        #endregion
    }
}