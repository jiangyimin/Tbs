using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class VehicleCache : IVehicleCache, IEventHandler<EntityChangedEventData<Vehicle>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleCache(
            ICacheManager cacheManager,
            IRepository<Vehicle> vehicleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _vehicleRepository = vehicleRepository;
            _abpSession = abpSession;
        }
        
        public virtual List<Vehicle> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedVehicles")
                .Get(cacheKey, () => _vehicleRepository.GetAll().Where($"DepotId = {depotId}").ToList());
        }

        public void HandleEvent(EntityChangedEventData<Vehicle> eventData)
        {
            var cacheKey = eventData.Entity.DepotId + "@" + (_abpSession.TenantId ?? 0);

            _cacheManager.GetCache("CachedVehicles").Remove(cacheKey);
        }
    }
}