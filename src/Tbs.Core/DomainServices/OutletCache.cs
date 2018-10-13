using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic.Core;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class OutletCache : IOutletCache
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Outlet> _outletRepository;

        public OutletCache(
            ICacheManager cacheManager,
            IRepository<Outlet> outletRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _outletRepository = outletRepository;
            _abpSession = abpSession;
        }
        
        public virtual List<Outlet> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedOutlets")
                .Get(cacheKey, () => _outletRepository.GetAllList(e => e.DepotId == null || e.DepotId == depotId));
        }
    }
}