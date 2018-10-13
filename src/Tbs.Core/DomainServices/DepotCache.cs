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
    public class DepotCache : IDepotCache, IEventHandler<EntityChangedEventData<Depot>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Depot> _depotRepository;

        public DepotCache(
            ICacheManager cacheManager,
            IRepository<Depot> depotRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _depotRepository = depotRepository;
            _abpSession = abpSession;
        }

        public virtual Depot Get(int depotId)
        {
            var cacheItem = GetOrNull(depotId);

            if (cacheItem == null)
            {
                throw new AbpException("There is no depot with given id: " + depotId);
            }

            return cacheItem;
        }
        
        public virtual Depot Get(string depotCn)
        {
            var cacheItem = GetOrNull(depotCn);

            if (cacheItem == null)
            {
                throw new AbpException("There is no depot with given depot cn: " + depotCn);
            }

            return cacheItem;
        }

        public virtual Depot GetOrNull(string depotCn)
        {
            var cacheKey = depotCn + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedDepotByCn")
                .Get(cacheKey, () => _depotRepository.FirstOrDefault(d => d.Cn == depotCn));

        }

        public Depot GetOrNull(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedDepot")
                .Get(cacheKey, () => _depotRepository.FirstOrDefault(depotId));
        }

        public List<Depot> GetList()
        {
            var cacheKey = (_abpSession.TenantId ?? 0).ToString();
            return _cacheManager.GetCache("CachedDepots")
                .Get(cacheKey, () => _depotRepository.GetAllList());
        }
        
        public void HandleEvent(EntityChangedEventData<Depot> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);

            _cacheManager.GetCache("CachedDepot").Remove(cacheKey);
            _cacheManager.GetCache("CachedDepotByCn").Remove(cacheKey);
        }
    }
}