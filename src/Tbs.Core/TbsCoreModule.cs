using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Tbs.Localization;
using Abp.Zero.Configuration;
using Tbs.MultiTenancy;
using Tbs.Authorization.Roles;
using Tbs.Authorization.Users;
using Tbs.Configuration;
using Tbs.Features;
using Tbs.Timing;
using Tbs.DomainServices;

namespace Tbs
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class TbsCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            TbsLocalizationConfigurer.Configure(Configuration.Localization);

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = TbsConsts.MultiTenancyEnabled;

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            // Add Feature and Setting Provider
            Configuration.Features.Providers.Add<TbsFeatureProvider>();
            Configuration.Settings.Providers.Add<TbsCoreSettingProvider>();

            //为特定的缓存配置有效期
            Configuration.Caching.Configure("CachedKeeper", cache =>
            {
                cache.DefaultSlidingExpireTime = System.TimeSpan.FromMinutes(10);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TbsCoreModule).GetAssembly());

            IocManager.Register<IArticleCache, ArticleCache>();
            IocManager.Register<IArticleTypeCache, ArticleTypeCache>();
            IocManager.Register<IWorkerCache, WorkerCache>();
            IocManager.Register<IDepotCache, DepotCache>();
            IocManager.Register<IRouteTypeCache, RouteTypeCache>();
            IocManager.Register<IRouteRoleCache, RouteRoleCache>();
            IocManager.Register<IVaultTypeCache, VaultTypeCache>();
            IocManager.Register<IVaultRoleCache, VaultRoleCache>();
            IocManager.Register<IVehicleCache, VehicleCache>();
            IocManager.Register<IOutletCache, OutletCache>();
            IocManager.Register<IWarehouseCache, WarehouseCache>();
            IocManager.Register<IVaultCache, VaultCache>();
            IocManager.Register<IDepotSigninCache, DepotSigninCache>();
            IocManager.Register<IKeeperCache, KeeperCache>();
            IocManager.Register<ITaskTypeCache, TaskTypeCache>();
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}