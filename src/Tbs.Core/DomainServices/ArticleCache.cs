using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class ArticleCache : IArticleCache
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Article> _articleRepository;

        public ArticleCache(
            ICacheManager cacheManager,
            IRepository<Article> articleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _articleRepository = articleRepository;
            _abpSession = abpSession;
        }
        
        public virtual List<Article> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedArticles")
                .Get(cacheKey, () => _articleRepository.GetAll().Where($"DepotId = {depotId}").ToList());
        }
    }
}