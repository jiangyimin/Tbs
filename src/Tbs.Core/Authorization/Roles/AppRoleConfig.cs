using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace Tbs.Authorization.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Admin,
                    MultiTenancySides.Host)
                );

            //Static tenant roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant)
                );
            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Dispatcher,
                    MultiTenancySides.Tenant)
                );
            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.AuxDispatcher,
                    MultiTenancySides.Tenant)
                );
            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Keeper,
                    MultiTenancySides.Tenant)
                );
        }
    }
}
