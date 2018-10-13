using Abp;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class TaskTypeCache : ITaskTypeCache, IEventHandler<EntityChangedEventData<TaskType>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TaskType> _taskTypeRepository;

        public TaskTypeCache(
            ICacheManager cacheManager,
            IRepository<TaskType> taskTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _taskTypeRepository = taskTypeRepository;
            _abpSession = abpSession;
        }

        public virtual TaskType Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no taskType with given id: " + id);
            }

            return cacheItem;
        }

        public TaskType GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedTaskType")
                .Get(cacheKey, () => _taskTypeRepository.FirstOrDefault(d => d.Id == id));
        }
        
        public void HandleEvent(EntityChangedEventData<TaskType> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);
            _cacheManager.GetCache("CachedTaskType").Remove(cacheKey);
        }
    }
}