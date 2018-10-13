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
    public class VaultRoleCache : IVaultRoleCache, IEventHandler<EntityChangedEventData<VaultRole>>
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<VaultRole> _vaultRoleRepository;

        public VaultRoleCache(
            ICacheManager cacheManager,
            IRepository<VaultRole> vaultRoleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _vaultRoleRepository = vaultRoleRepository;
            _abpSession = abpSession;
        }

        public virtual VaultRole Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no vaultType with given id: " + id);
            }

            return cacheItem;
        }

        public VaultRole GetOrNull(int id)
        {
            var cacheKey = id + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedVaultRole")
                .Get(cacheKey, () => _vaultRoleRepository.FirstOrDefault(d => d.Id == id));
        }

        public List<VaultRole> GetVaultRoles(int vaultTypeId)
        {
            var cacheKey = vaultTypeId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedVaultRoles")
                .Get(cacheKey, () => _vaultRoleRepository.GetAllList(d => d.VaultTypeId == vaultTypeId));
        }
        
        
        public void HandleEvent(EntityChangedEventData<VaultRole> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (_abpSession.TenantId ?? 0);
            _cacheManager.GetCache("CachedVaultRole").Remove(cacheKey);
        }
    }
}