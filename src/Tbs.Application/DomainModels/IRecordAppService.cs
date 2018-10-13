using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface IRecordAppService : IApplicationService
    {
        Task<List<ArticleListDto>> GetArticleList(int depotId, string sorting);
        void LendArticles(int depotId, int routeId, int routeWorkerId, int workerId, string workerCn, string workerName, List<int> ids, string remark);
        void ReturnArticles(int routeId, int routeWorkerId, string remark);       

        string DaySettle(int depotId, DateTime carryoutDate, int settleId);

        bool ArticleLendedToday(int articleId, out string msg);

        Task<List<ArticleRecordSearchDto>> SearchByDay(int depotId, DateTime theDay);
        Task<List<ArticleRecordSearchDto>> SearchByArticleId(int depotId, int articleId, DateTime begin, DateTime end);
    }
}
