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
    public class VaultCache : IVaultCache
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Vault> _vaultRepository;

        public VaultCache(
            ICacheManager cacheManager,
            IRepository<Vault> vaultRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _vaultRepository = vaultRepository;
            _abpSession = abpSession;
        }
        
        public virtual List<Vault> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedVaults")
                .Get(cacheKey, () => _vaultRepository.GetAll().Where($"DepotId = {depotId}").ToList());
        }

    }
}