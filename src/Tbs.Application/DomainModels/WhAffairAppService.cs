using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Tbs.DomainModels.Dto;
using Tbs.Timing;
using Abp.Linq;

namespace Tbs.DomainModels
{
    class Settle {
        public int CountTotal { get; set; }
        public int CountWarn { get; set; }

        public string CheckResult { get; set; }
   }

    public class WhAffairAppService : TbsAppServiceBase, IWhAffairAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<WhAffair> _affairRepository;
        private readonly IRepository<WhAffairWorker> _workerRepository;
        private readonly IRepository<DaySettle> _settleRepository;

        public WhAffairAppService(IRepository<WhAffair> affairRepository, IRepository<WhAffairWorker> workerRepository,
                IRepository<DaySettle> settleRepository)
        {
            _affairRepository = affairRepository;
            _workerRepository = workerRepository;
            _settleRepository = settleRepository;
        }


        public List<WhAffairDto> GetAffairs(int depotId, DateTime carryoutDate, string sorting)
        {
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            var o = _affairRepository.GetAll().Where($"DepotId={depotId} AND CarryoutDate=\"{dateString}\"").OrderBy(sorting).ToList();
            return new List<WhAffairDto>(o.Select(ObjectMapper.Map<WhAffairDto>));
        }

        public List<WhAffairDto> GetAffairsActive(int depotId, DateTime carryoutDate, string whName, string sorting)
        {
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            var o = _affairRepository.GetAll().Where($"DepotId={depotId} AND CarryoutDate=\"{dateString}\"").OrderBy(sorting).ToList();
            return new List<WhAffairDto>(o.Select(ObjectMapper.Map<WhAffairDto>).Where(e=>e.Status != "安排" && e.WhName == whName));
        }

        public async Task<WhAffairDto> Insert(WhAffairDto input)
        {
                var entity = ObjectMapper.Map<WhAffair>(input);
                await _affairRepository.InsertAsync(entity);
                CurrentUnitOfWork.SaveChanges();
                return ObjectMapper.Map<WhAffairDto>(entity);
        }

        public async Task<WhAffairDto> Update(WhAffairDto input)
        {
            var entity = _affairRepository.Get(input.Id);
            ObjectMapper.Map<WhAffairDto, WhAffair>(input, entity);
            await _affairRepository.UpdateAsync(entity);
            return ObjectMapper.Map<WhAffairDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _affairRepository.DeleteAsync(id);
        }

        public async Task<int> CreateFrom(int depotId, DateTime carryoutDate, DateTime fromDate)
        {
            var list = _affairRepository.GetAllList(e=>e.DepotId == depotId && e.CarryoutDate == fromDate);
            foreach (WhAffair a in list)
            {
                WhAffair affair = new WhAffair();
                affair.DepotId = depotId;
                affair.CarryoutDate = carryoutDate;
                affair.Status = "安排";
                affair.WhName = a.WhName;
                affair.StartTime = a.StartTime;
                affair.EndTime = a.EndTime;
                affair.Remark = a.Remark;

                var workers =  _workerRepository.GetAllList(e => e.WhAffairId == a.Id);
                foreach (WhAffairWorker w in workers)
                {
                    WhAffairWorker worker = new WhAffairWorker();
                    worker.WorkerId = w.WorkerId;
                    worker.WorkerCn = w.WorkerCn;
                    worker.WorkerName = w.WorkerName;
                    if (affair.Workers == null)
                        affair.Workers = new List<WhAffairWorker>();
                    affair.Workers.Add(worker);
                }

                await _affairRepository.InsertAsync(affair);
            }

            return list.Count;
        }

        public async Task<int> Activate(int depotId, DateTime carryoutDate, string status)
        {
            var o = _affairRepository.GetAllList(e => e.DepotId==depotId && e.CarryoutDate==carryoutDate && e.Status == "安排");
            foreach (WhAffair a in o)
            {
                a.Status = status;
                await _affairRepository.UpdateAsync(a);
            }
            return o.Count;
        }

        public KeyValuePair<string, string> MatchWorker(Worker worker, int affairId)
        {
            WhAffair affair = _affairRepository.Get(affairId);
            var workers = _workerRepository.GetAllList(e=>e.WhAffairId == affairId);
            foreach (WhAffairWorker whWorker in workers)
            {
                if (whWorker.WorkerId == worker.Id)
                    return new KeyValuePair<string, string>("ok", $"{whWorker.WorkerName}验证正确");                     
            }
            return new KeyValuePair<string, string>("error", "此任务未安排此人"); 
        }
        
        public KeyValuePair<string, string> CheckWorker(Worker worker, int affairId)
        {
            WhAffair affair = _affairRepository.Get(affairId);
                
            int dist = TimeUtil.DistToNow(DateTime.Today, affair.StartTime);
            if (dist < 0 && dist < -15)
                return new KeyValuePair<string, string>("error", "现在未到任务开始前15分钟");

            var workers = _workerRepository.GetAllList(e=>e.WhAffairId == affairId);
            foreach (WhAffairWorker whWorker in workers)
            {
                if (whWorker.WorkerId == worker.Id)
                {                    
                    if (whWorker.CheckIn == null) {
                        whWorker.CheckIn = DateTime.Now;
                        _workerRepository.Update(whWorker);
                        return new KeyValuePair<string, string>("ok", $"{whWorker.WorkerName}签入成功");                       
                    }
                    else
                    {
                        if (whWorker.CheckOut == null)
                        {
                            whWorker.CheckOut = DateTime.Now;
                            _workerRepository.Update(whWorker);
                            return new KeyValuePair<string, string>("ok", $"{whWorker.WorkerName}签出成功");    
                        }  
                        else
                        {
                            return new KeyValuePair<string, string>("error", $"{whWorker.WorkerName}已签出");      
                        }                 
                    }
                }
            }
            return new KeyValuePair<string, string>("error", "此任务未安排此人"); 
        }

        public string DaySettle(int depotId, DateTime carryoutDate, int settleId) 
        {
            Dictionary<string, Settle> dsDict = new Dictionary<string, Settle>();
            foreach (var a in _affairRepository.GetAllList(a => a.DepotId == depotId && a.CarryoutDate == carryoutDate))
            {
                string name = a.WhName + (a.Remark == null ? null : a.Remark.Split()[0]);
                if (!dsDict.ContainsKey(name))
                    dsDict[name] = new Settle();
                Settle settle = dsDict[name];
                
                foreach (var w in _workerRepository.GetAllList(w => w.WhAffairId == a.Id).ToList())
                {                   
                    settle.CountTotal++;
                    if (!w.CheckIn.HasValue || !w.CheckOut.HasValue) {
                        settle.CheckResult += w.WorkerName + " ";
                        settle.CountWarn++;
                    }
                }
            }

            string settleResult = null;
            string checkResult = null;
            int checkCount = 0;
            foreach (KeyValuePair<string, Settle> kv in dsDict)
            {
                checkCount += kv.Value.CountTotal;
                checkResult += kv.Value.CheckResult;
                settleResult += $"【{kv.Key}安排{kv.Value.CountTotal}人上岗";
                settleResult += kv.Value.CountWarn > 0 ?  $"({kv.Value.CountWarn}人异常)】" : "】";
            }
            if (settleId > 0)
            {
                var s = _settleRepository.Get(settleId);
                s.Message += $"{settleResult}";                
                _settleRepository.Update(s);
                return settleResult;
            }
            else
            {
                return checkResult == null ? checkCount.ToString() : checkResult + "未正常签入/签出";
            }
        }

        #region Task
        public List<WhAffairWorkerDto> GetAffairWorkers(int id, string sorting)
        {
            var o = _workerRepository.GetAll().Where(e => e.WhAffairId == id).OrderBy(sorting).ToList();

            return new List<WhAffairWorkerDto>(o.Select(ObjectMapper.Map<WhAffairWorkerDto>).ToList());

        }
        public async Task<WhAffairWorkerDto> UpdateSon(WhAffairWorkerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<WhAffairWorkerDto, WhAffairWorker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WhAffairWorkerDto>(entity);
        }

        public async Task<WhAffairWorkerDto> InsertSon(WhAffairWorkerDto input)
        {
            var entity = ObjectMapper.Map<WhAffairWorker>(input);

            await _workerRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WhAffairWorkerDto>(entity);
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
                foreach (var w in _workerRepository.GetAllList(w=>w.WhAffairId == r.Id))
                {
                    string key = w.WorkerCn + " " + w.WorkerName + '|' + r.WhName;
                    if (!dict.ContainsKey(key)) 
                        dict[key] = new CommonStat(2);
                
                    CommonStat cs = dict[key];

                    if (!w.CheckIn.HasValue)
                        cs.Qtums[0] += 1;
                    if (!w.CheckOut.HasValue)
                        cs.Qtums[1] += 1;
                           
                    cs.DtHS.Add(r.CarryoutDate);
                }
            }

            var dtos = new List<AffairWorkerStatDto>();
            foreach (var item in dict.ToList().OrderBy(a => a.Key))
            {
                string[] keys = item.Key.Split('|');
                AffairWorkerStatDto dto = new AffairWorkerStatDto(keys[0]);
                dto.Name = keys[1];
                CommonStat cs = item.Value;
                dto.DayCount = cs.DtHS.Count();
                dto.NoCheckInCount = cs.Qtums[0];
                dto.NoCheckOutCount = cs.Qtums[1];
                dtos.Add(dto);
            }
            return dtos;
        }

        #endregion

        #region util
        private List<WhAffair> GetWhAffairs(DateTime carryoutDate, string sorting, string depotId)
        {
            return _affairRepository.GetAll().OrderBy(sorting).ToList();
        }

        #endregion
    }
}