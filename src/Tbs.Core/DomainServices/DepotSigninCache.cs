using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public class DepotSigninCache : IDepotSigninCache
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<DepotSignin> _signinRepository;

        public DepotSigninCache(
            ICacheManager cacheManager,
            IRepository<DepotSignin> signinRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _signinRepository = signinRepository;
            _abpSession = abpSession;
        }
        
        public virtual List<DepotSignin> GetList(int depotId)
        {
            var cacheKey = depotId + "@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache("CachedDepotSignins")
                .Get(cacheKey, () => _signinRepository.GetAll().Where($"DepotId = {depotId}").ToList());
        }
    }
}