using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class ArticleTypeCache : IArticleTypeCache, IEventHandler<EntityChangedEventData<ArticleType>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<ArticleType> _articleTypeRepository;

        public ArticleTypeCache(
            ICacheManager cacheManager,
            IRepository<ArticleType> articleTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _articleTypeRepository = articleTypeRepository;
            _abpSession = abpSession;
        }

        public virtual ArticleType Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no articleType with given id: " + id);
            }

            return cacheItem;
        }

        public ArticleType GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedArticleType")
                .Get(cacheKey, () => _articleTypeRepository.FirstOrDefault(d => d.Id == id));
        }
        
        public void HandleEvent(EntityChangedEventData<ArticleType> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);
            _cacheManager.GetCache("CachedArticleType").Remove(cacheKey);
        }
    }
}