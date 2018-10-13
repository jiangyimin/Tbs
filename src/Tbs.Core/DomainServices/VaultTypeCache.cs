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
    public class VaultTypeCache : IVaultTypeCache, IEventHandler<EntityChangedEventData<VaultType>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<VaultType> _vaultTypeRepository;
        private readonly IRepository<VaultRole> _vaultRoleRepository;

        public VaultTypeCache(
            ICacheManager cacheManager,
            IRepository<VaultType> vaultTypeRepository,
            IRepository<VaultRole> vaultRoleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _vaultTypeRepository = vaultTypeRepository;
            _vaultRoleRepository = vaultRoleRepository;
            _abpSession = abpSession;
        }

        public VaultType GetFirst()
        {
            return _vaultTypeRepository.GetAllList()[0];
        }

        public virtual VaultType Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no vaultType with given id: " + id);
            }

            return cacheItem;
        }

        public VaultType GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedVaultType")
                .Get(cacheKey, () => GetVaultTypeOrNull(id));
        }
        
        protected virtual VaultType GetVaultTypeOrNull(int id)
        {
            var type =  _vaultTypeRepository.FirstOrDefault(d => d.Id == id);
            
            if (type.Roles == null)
            {
                type.Roles = _vaultRoleRepository.GetAllList();
            }
            return type;
        }


        public void HandleEvent(EntityChangedEventData<VaultType> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);
            _cacheManager.GetCache("CachedVaultType").Remove(cacheKey);
        }
    }
}