using System;
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
    public class KeeperCache : IKeeperCache
    {
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WhAffair> _affairRepository;
        private readonly IRepository<WhAffairWorker> _workerRepository;

        public KeeperCache(
            ICacheManager cacheManager,
            IRepository<WhAffair> affairRepository,
            IRepository<WhAffairWorker> workerRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _affairRepository = affairRepository;
            _workerRepository = workerRepository;
            _abpSession = abpSession;
        }
        
        public virtual string Get(int depotId, DateTime carryoutDate, string whName)
        {
        //     var cacheKey = whName + "@" + depotId + "@" + (_abpSession.TenantId ?? 0);
        //     return _cacheManager.GetCache("CachedKeeper")
        //         .Get(cacheKey, () => GetKeeperInfo(depotId, carryoutDate, whName));
        // }

        // private string GetKeeperInfo(int depotId, DateTime carryoutDate, string whName)
        // {
            string keeperInfo = null;
            var o = _affairRepository.GetAll().Where(a=>a.DepotId==depotId && a.CarryoutDate==carryoutDate && a.WhName == whName && a.Status != "安排").OrderBy(a => a.StartTime).ToList();
            foreach (WhAffair affair in o)
            {
                if (Tbs.Timing.TimeUtil.IsIn(carryoutDate, affair.StartTime, affair.EndTime))
                {
                    foreach(WhAffairWorker w in _workerRepository.GetAll().Where(e => e.WhAffairId == affair.Id).ToList())
                        keeperInfo += $"{w.WorkerCn}{w.WorkerName} ";
                    
                    keeperInfo = string.Format("{0},{1}", affair.Id, keeperInfo);
                }
            }
            return keeperInfo;
        }
    }
}