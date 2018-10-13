using System.Collections.Generic;
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
    public class RouteRoleCache : IRouteRoleCache, IEventHandler<EntityChangedEventData<RouteRole>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<RouteRole> _routeRoleRepository;

        public RouteRoleCache(
            ICacheManager cacheManager,
            IRepository<RouteRole> routeRoleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _routeRoleRepository = routeRoleRepository;
            _abpSession = abpSession;
        }

        public virtual RouteRole Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no routeType with given id: " + id);
            }

            return cacheItem;
        }

        public RouteRole GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedRouteRole")
                .Get(cacheKey, () => _routeRoleRepository.FirstOrDefault(d => d.Id == id));
        }
        
        public List<RouteRole> GetRouteRoles(int routeTypeId)
        {
            var cacheKey = routeTypeId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedRouteRoles")
                .Get(cacheKey, () => _routeRoleRepository.GetAllList(d => d.RouteTypeId == routeTypeId));
        }
        
        public void HandleEvent(EntityChangedEventData<RouteRole> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);
            _cacheManager.GetCache("CachedRouteRole").Remove(cacheKey);
        }
    }
}