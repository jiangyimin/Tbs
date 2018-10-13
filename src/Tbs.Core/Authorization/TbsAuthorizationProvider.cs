using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Tbs.Authorization
{
    public class TbsAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Host permissions
            var hostPages = context.GetPermissionOrNull(PermissionNames.HostPages);
            if (hostPages == null)
            {
                hostPages = context.CreatePermission(PermissionNames.HostPages, new FixedLocalizableString("租主"), null, MultiTenancySides.Host);
            }

            var tenantAdminPages = context.GetPermissionOrNull(PermissionNames.AdminPages);
            if (tenantAdminPages == null)
            {
                tenantAdminPages = context.CreatePermission(PermissionNames.AdminPages, new FixedLocalizableString("租户管理"), null, MultiTenancySides.Tenant);
                tenantAdminPages.CreateChildPermission(PermissionNames.Admin_Setup, new FixedLocalizableString("租户配置"), null, MultiTenancySides.Tenant);
                tenantAdminPages.CreateChildPermission(PermissionNames.Admin_Data, new FixedLocalizableString("租户数据"), null, MultiTenancySides.Tenant);                
                tenantAdminPages.CreateChildPermission(PermissionNames.Admin_Manage, new FixedLocalizableString("租户管理"), null, MultiTenancySides.Tenant);                
            }

            // Dispatcher permissions
            var dispatcherPages = context.GetPermissionOrNull(PermissionNames.DispatcherPages);
            if (dispatcherPages == null)
            {
                dispatcherPages = context.CreatePermission(PermissionNames.DispatcherPages, new FixedLocalizableString("调度"), null, MultiTenancySides.Tenant);
            }

            // SubDispatcher permissions
            var auxDispatcherPages = context.GetPermissionOrNull(PermissionNames.AuxDispatcherPages);
            if (auxDispatcherPages == null)
            {
                auxDispatcherPages = context.CreatePermission(PermissionNames.AuxDispatcherPages, new FixedLocalizableString("辅助调度"), null, MultiTenancySides.Tenant);
            }

            // Keeper permissions
            var keeperPages = context.GetPermissionOrNull(PermissionNames.KeeperPages);
            if (keeperPages == null)
            {
                keeperPages = context.CreatePermission(PermissionNames.KeeperPages, new FixedLocalizableString("库房"), null, MultiTenancySides.Tenant);
            }

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TbsConsts.LocalizationSourceName);
        }
    }
}
