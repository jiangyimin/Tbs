using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Tbs.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using System;
using System.Threading.Tasks;
using Abp.Linq;

namespace Tbs.DomainModels
{
    public class RecordAppService : TbsAppServiceBase, IRecordAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<ArticleRecord> _recordRepository;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<RouteWorker> _routeWorkerRepository;
        private readonly IRepository<DaySettle> _settleRepository;


        public RecordAppService(IRepository<ArticleRecord> recordRepository, IRepository<Article> articleRepository,        
                IRepository<Route> routeRepository, IRepository<RouteWorker> routeWorkerRepository, 
                IRepository<DaySettle> settleRepository)
        {
            _recordRepository = recordRepository;
            _articleRepository = articleRepository;
            _routeRepository = routeRepository;
            _routeWorkerRepository = routeWorkerRepository;
            _settleRepository = settleRepository;
        }

        public async Task<List<ArticleListDto>> GetArticleList(int depotId, string sorting)
        {
            var query = _articleRepository.GetAll().Where(a => a.DepotId == depotId).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new List<ArticleListDto>(entities.Select(ObjectMapper.Map<ArticleListDto>).ToList());
            foreach (var dto in dtos)
            {
                dto.ArticleTypeName = DomainManager.GetArticleType(dto.ArticleTypeId).Name;
                dto.ArticleRecordInfo = GetArticleRecordInfo(dto.ArticleRecordId);
            }
            return dtos;
        }

        public void LendArticles(int depotId, int routeId, int routeWorkerId, int workerId, string workerCn, string workerName, List<int> ids, string remark)
        {
            Route r = _routeRepository.Get(routeId);
            r.Status = "领物";
            string info = r.RouteName;
            if (r.VehicleId.HasValue) {
                var vehicle = DomainManager.GetVehicle(depotId, r.VehicleId.Value);
                if (vehicle != null)
                    info += $",{vehicle.Cn} {vehicle.License}";
            }

            RouteWorker rw = _routeWorkerRepository.Get(routeWorkerId);
            if (!string.IsNullOrEmpty(rw.RecordList)) 
                return;

            string recordList = null;
            foreach (int id in ids)
            {               
                ArticleRecord record = new ArticleRecord() {
                    DepotId = depotId,
                    ArticleId = id,
                    WorkerId = workerId, WorkerCn = workerCn, WorkerName = workerName,
                    LendTime = DateTime.Now,
                    Remark = $"【{info}】【领用管理:{remark}】"
                };

                _recordRepository.Insert(record);
                CurrentUnitOfWork.SaveChanges();
                recordList += $"{record.Id} ";
                Article article = _articleRepository.Get(id);
                article.ArticleRecordId = record.Id;
            }
        
            rw.LendTime = DateTime.Now;
            if (string.IsNullOrEmpty(rw.RecordList)) {
                rw.RecordList +=  recordList.TrimEnd();
            }
            else
            {
                rw.RecordList += " " + recordList.TrimEnd();
            } 
        }

        public void ReturnArticles(int routeId, int routeWorkerId, string remark)
        {
            Route r = _routeRepository.Get(routeId);
            r.Status = "还物";
            RouteWorker rw = _routeWorkerRepository.Get(routeWorkerId);
            rw.ReturnTime = DateTime.Now;
            foreach (string id in rw.RecordList.Split())
            {
                ArticleRecord record = _recordRepository.Get(int.Parse(id));
                record.ReturnTime = DateTime.Now;
                record.Remark += $"【归还管理:{remark}】";
            }
        }

        public string DaySettle(int depotId, DateTime carryoutDate, int settleId) 
        {
            int countTotal = 0; 
            int countWarn = 0;
            foreach (var a in _articleRepository.GetAllList(a => a.DepotId == depotId))
            {
                if (a.ArticleRecordId.HasValue)
                {
                    var r = _recordRepository.Get(a.ArticleRecordId.Value);
                    if (r.LendTime.Date == carryoutDate) countTotal++;
                    if (!r.ReturnTime.HasValue) countWarn++;
                }           
            }

            string settleResult = $"领用了{countTotal}个物品";
            if (countWarn > 0) 
                settleResult += $"({countWarn}个未归还)";
            else
                settleResult += ", 已全部归还";


            if (settleId > 0)
            {
                var s = _settleRepository.Get(settleId);               
                s.Message += $"【{settleResult}】";
                return settleResult;
            }
            else 
            {
                return countWarn == 0 ? countTotal.ToString() : settleResult;
            }
        }

        public bool ArticleLendedToday(int articleId, out string msg) 
        {
            msg = null;
            var a = _articleRepository.Get(articleId);
            if (!a.ArticleRecordId.HasValue) return false;

            var record = _recordRepository.Get(a.ArticleRecordId.Value);
            if (!record.ReturnTime.HasValue && record.LendTime.Date == DateTime.Today)
            {
                msg = $"此物今日已被{record.WorkerName}领走";
                return true;
            }
            else {
                return false;
            }
        }

        public async Task<List<ArticleRecordSearchDto>> SearchByDay(int depotId, DateTime theDay)
        {
            var query = _recordRepository.GetAll().Where(a => a.DepotId == depotId && a.LendTime.Date == theDay);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return MapToDto(entities);
 
        }
        public async Task<List<ArticleRecordSearchDto>> SearchByArticleId(int depotId, int articleId, DateTime begin, DateTime end)
        {
            var query = _recordRepository.GetAll().Where(a => a.DepotId == depotId && a.ArticleId == articleId &&
                a.LendTime.Date >= begin && a.LendTime.Date <= end);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return MapToDto(entities);
        }

        #region util

        private string GetArticleRecordInfo(int? recordId)
        {
            if (!recordId.HasValue) return null;

            var r = _recordRepository.Get(recordId.Value);

            string l = $"领用人：{r.WorkerName} 领用时间：{r.LendTime.ToString("yyyy-MM-dd HH:mm")} ";
            string h = r.ReturnTime.HasValue ? $"【归还时间：{r.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm")}】" : "【未还】";
            return l+h;
        }

        private List<ArticleRecordSearchDto> MapToDto(List<ArticleRecord> entities)
        {
            List<ArticleRecordSearchDto> dtos = new List<ArticleRecordSearchDto>();
            foreach (var r in entities)
            {
                var a = DomainManager.GetArticle(r.DepotId, r.ArticleId);
                dtos.Add(new ArticleRecordSearchDto() {
                    Worker = r.WorkerCn + " " + r.WorkerName,
                    Article = a != null ? a.Cn + " " + a.Name : null,
                    LendTime = r.LendTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ReturnTime =  r.ReturnTime.HasValue ? r.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    Remark = r.Remark
                });
            }
            return dtos;
        }
        #endregion
    }
}