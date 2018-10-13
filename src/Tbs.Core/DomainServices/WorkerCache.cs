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
    public class WorkerCache : IWorkerCache, IEventHandler<EntityDeletedEventData<Worker>>, IEventHandler<EntityCreatedEventData<Worker>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Worker> _workerRepository;

        public WorkerCache(
            ICacheManager cacheManager,
            IRepository<Worker> workerRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _workerRepository = workerRepository;
            _abpSession = abpSession;
        }
        public virtual List<Worker> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedWorkers")
                .Get(cacheKey, () => _workerRepository.GetAllList(e=>e.DepotId==depotId));
        }

        public virtual Worker Get(string workerCn)
        {
            var cacheItem = GetOrNull(workerCn);

            if (cacheItem == null)
            {
                throw new AbpException("There is no worker with given worker cn: " + workerCn);
            }

            return cacheItem;
        }

        public virtual Worker GetOrNull(string workerCn)
        {
            var cacheKey = workerCn + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedWorker")
                .Get(cacheKey, () => GetWorkerOrNull(workerCn));
        }

        protected virtual Worker GetWorkerOrNull(string workerCn)
        {
            return _workerRepository.FirstOrDefault(d => d.Cn == workerCn);
        }

        public void HandleEvent(EntityDeletedEventData<Worker> eventData)
        {
            var cacheKey = eventData.Entity.DepotId + "@" + (_abpSession.TenantId ?? 0);

            _cacheManager.GetCache("CachedWorkers").Remove(cacheKey);
        }
        public void HandleEvent(EntityCreatedEventData<Worker> eventData)
        {
            var cacheKey = eventData.Entity.DepotId + "@" + (_abpSession.TenantId ?? 0);

            _cacheManager.GetCache("CachedWorkers").Remove(cacheKey);
        }
    }
}