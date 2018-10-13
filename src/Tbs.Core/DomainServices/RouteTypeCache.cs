using Abp;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class RouteTypeCache : IRouteTypeCache, IEventHandler<EntityChangedEventData<RouteType>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<RouteType> _routeTypeRepository;

        public RouteTypeCache(
            ICacheManager cacheManager,
            IRepository<RouteType> routeTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _routeTypeRepository = routeTypeRepository;
            _abpSession = abpSession;
        }

        public RouteType GetFirst()
        {
            return _routeTypeRepository.GetAllList()[0];
        }

        public virtual RouteType Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no routeType with given id: " + id);
            }

            return cacheItem;
        }

        public RouteType GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedRouteType")
                .Get(cacheKey, () => _routeTypeRepository.FirstOrDefault(d => d.Id == id));
        }
        
        public void HandleEvent(EntityChangedEventData<RouteType> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);
            _cacheManager.GetCache("CachedRouteType").Remove(cacheKey);
        }
    }
}