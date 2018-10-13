using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class WarehouseCache : IWarehouseCache
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Warehouse> _warehouseRepository;

        public WarehouseCache(
            ICacheManager cacheManager,
            IRepository<Warehouse> warehouseRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _warehouseRepository = warehouseRepository;
            _abpSession = abpSession;
        }
        
        public virtual List<Warehouse> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedWarehouses")
                .Get(cacheKey, () => _warehouseRepository.GetAll().Where($"DepotId = {depotId}").ToList());
        }

        public virtual Warehouse Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no warehouse with given id: " + id);
            }

            return cacheItem;
        }

       public virtual Warehouse GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedWarehouse")
                .Get(cacheKey, () => _warehouseRepository.FirstOrDefault(id));
        }
    }
}