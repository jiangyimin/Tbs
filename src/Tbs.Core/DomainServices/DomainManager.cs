using System;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Tbs.Authorization.Users;
using Tbs.DomainModels;
using Tbs.MultiTenancy;

namespace Tbs.DomainServices
{
    /// <summary>
    /// Depot manager.
    /// Implements domain logic for depot.
    /// </summary>
    public class DomainManager : IDomainService
    {
        public IAbpSession AbpSession { protected get; set; }
        public ICacheManager CacheManager { protected get; set; }
        public TenantManager TenantManager { protected get; set; }
        public UserManager UserManager { protected get; set; }

        private readonly IRepository<ArticleRecord> _recordRepository;
        private readonly IArticleCache _articleCache;
        private readonly IArticleTypeCache _articleTypeCache;
        private readonly IWorkerCache _workerCache;
        private readonly IDepotCache _depotCache;
        private readonly IRouteTypeCache _routeTypeCache;
        private readonly IRouteRoleCache _routeRoleCache;
        private readonly IVaultTypeCache _vaultTypeCache;
        private readonly IVaultRoleCache _vaultRoleCache;
        private readonly IVehicleCache _vehicleCache;
        private readonly IOutletCache _outletCache;
        private readonly IWarehouseCache _warehouseCache;
        private readonly IVaultCache _vaultCache;
        private readonly IKeeperCache _keeperCache;
        private readonly ITaskTypeCache _taskTypeCache;

        public DomainManager(
            IRepository<ArticleRecord> recordRepository,
            IArticleCache articleCache,
            IArticleTypeCache articleTypeCache, 
            IWorkerCache workerCache,
            IDepotCache depotCache, 
            IRouteTypeCache routeTypeCache,
            IRouteRoleCache routeRoleCache,
            IVaultTypeCache vaultTypeCache,
            IVaultRoleCache vaultRoleCache,
            IVehicleCache vehicleCache, 
            IOutletCache outletCache, 
            IWarehouseCache warehouseCache, 
            IVaultCache vaultCache,
            IKeeperCache keeperCache, 
            ITaskTypeCache taskTypeCache)
        {
            _recordRepository = recordRepository;
            _articleCache = articleCache;
            _articleTypeCache = articleTypeCache;
            _workerCache = workerCache;
            _depotCache = depotCache;
            _routeTypeCache = routeTypeCache;
            _routeRoleCache = routeRoleCache;
            _vaultTypeCache = vaultTypeCache;
            _vaultRoleCache = vaultRoleCache;
            _vehicleCache = vehicleCache;
            _outletCache = outletCache;
            _warehouseCache = warehouseCache;
            _vaultCache = vaultCache;
            _keeperCache = keeperCache;
            _taskTypeCache = taskTypeCache;
        }
        
        public string GetUserDisplayInfo()
        {
            var user = GetCurrentUser();

            if (user.DepotSide)
            {
                var worker = _workerCache.GetOrNull(user.UserName);
                if (worker != null)
                {
                    var depot = _depotCache.GetOrNull(worker.DepotId);
                    return $"欢迎 {worker.Cn} {worker.Name} ({depot.Name} {user.WhName})";
                }
                else 
                {
                    return user.Name;
                }
            }
            else
            {
                return user.Name;
            }
        }

        public List<Depot> GetCurrentUserDepots()
        {
            List<Depot> depots = new List<Depot>();
            if (GetCurrentUser().DepotSide) {
                depots.Add(GetDepotById(GetDepotId()));
            }
            else {
                depots = _depotCache.GetList();
            }
            return depots;
        }
        
        public int GetDepotId()
        {
            var worker = GetUserWorker();
            if (worker == null)
                throw new UserFriendlyException("当前用户编号应该存在于人员数据表中"); 
            return _depotCache.GetOrNull(worker.DepotId).Id;
        }

        public Depot GetDepotById(int depotId)
        {
            return _depotCache.Get(depotId);
        }

        public Depot GetDepotByCn(string depotCn)
        {
            return _depotCache.GetOrNull(depotCn);
        }
        public Worker GetUserWorker()
        {
            var user = GetCurrentUser();
            if (!user.DepotSide) 
                throw new UserFriendlyException("当前用户应该是操作端"); 
            
            return _workerCache.GetOrNull(user.UserName);
        }

        public Worker GetWorkerByCn(string workerCn)
        {
            return _workerCache.GetOrNull(workerCn);
        }
        
        public Worker GetWorkerById(int depotId, int workerId)
        {
            foreach (var worker in GetWorkers(depotId))
            {
                if (worker.Id == workerId) return worker;
            }
            return null;                    //_workerCache.GetOrNull(workerCn);            
        }
        
        public User GetCurrentUser()
        {
            return UserManager.GetUserByIdAsync(AbpSession.UserId.Value).Result;
            // var cachekey = AbpSession.UserId + "@" + (AbpSession.TenantId ?? 0);
            // //从缓存中获取
            // return CacheManager
            //         .GetCache("CachedUsers")
            //         .Get(cachekey, () => UserManager.GetUserByIdAsync(AbpSession.UserId.Value).Result) as User;
        }

        public int GetTenantIdByName(string tenancyName)
        {
            var tenant = TenantManager.FindByTenancyNameAsync(tenancyName).Result;
            if (tenant == null)
                return 0;
            else
                return tenant.Id;
        }
        
        public List<Article> GetArticles(int depotId) {
            return _articleCache.GetList(depotId);
        }

        public Article GetArticle(int depotId, int articleId)
        {
            foreach (var a in GetArticles(depotId))
            {
                if (a.Id == articleId)
                    return a;
            }
            return null;
        }
        public ArticleType GetArticleType(int typeId)
        {
            return _articleTypeCache.Get(typeId);
        }

        public RouteType GetRouteType(int id)
        {
            return _routeTypeCache.GetOrNull(id);
        }

        public List<RouteRole> GetRouteRoles()
        {
            return _routeRoleCache.GetRouteRoles(_routeTypeCache.GetFirst().Id);
        }
        public List<RouteRole> GetRouteRoles(int id)
        {
            return _routeRoleCache.GetRouteRoles(id);
        }

        public RouteRole GetRouteRole(int id)
        {
            return _routeRoleCache.GetOrNull(id);
        }
        public VaultType GetVaultType(int id)
        {
            return _vaultTypeCache.GetOrNull(id);
        }

        public List<VaultRole> GetVaultRoles(int id)
        {
            return _vaultRoleCache.GetVaultRoles(id);
        }

        public List<VaultRole> GetVaultRoles()
        {
            return _vaultRoleCache.GetVaultRoles(_vaultTypeCache.GetFirst().Id);
        }

        public VaultRole GetVaultRole(int id)
        {
            return _vaultRoleCache.GetOrNull(id);
        }
        public List<Worker> GetWorkers(int depotId)
        {
            return _workerCache.GetList(depotId);
        }
        public List<Vehicle> GetVehicles(int depotId)
        {
            return _vehicleCache.GetList(depotId);
        }

        public Vehicle GetVehicle(int depotId, int id)
        {
            foreach (Vehicle v in GetVehicles(depotId))
            {
                if (v.Id == id) return v;
            }
            return null;
        }
        public List<Outlet> GetOutlets(int depotId)
        {
            return _outletCache.GetList(depotId);
        }
        public List<Warehouse> GetWarehouses(int depotId)
        {
            return _warehouseCache.GetList(depotId);
        }
        public List<Vault> GetVaults(int depotId)
        {
            return _vaultCache.GetList(depotId);
        }

        public TaskType GetTaskType(int id)
        {
            return _taskTypeCache.GetOrNull(id);
        }

        public string GetKeepersInfo(int depotId, DateTime carryoutDate, string whName)
        {
            return _keeperCache.Get(depotId, carryoutDate, whName);
        }

        public string GetRecordListDetail(string recordList)
        {
            if (string.IsNullOrEmpty(recordList)) return null;

            string list = null;
            int count = 0;
            foreach (string id in recordList.Split())
            {
                int recordId = 0;
                if (int.TryParse(id, out recordId))
                {
                    var record = _recordRepository.Get(recordId);
                    if (record != null)
                    {
                        count++;
                        Article a = GetArticle(record.DepotId, record.ArticleId);
                        list += $"{count}){a.Name} ";
                    }
                }
            }

            return $"共{recordList.Split().Length}物品：{list.TrimEnd()}";
        }
    }
}