using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Localization;
using Tbs.Authorization;
using Tbs.Configuration;
using Tbs.Settings;

namespace Tbs.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class TbsNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                // Host
                .AddItem(new MenuItemDefinition("HostPages", new FixedLocalizableString("配置"), icon: "fa fa-home", requiredPermissionName: PermissionNames.HostPages)
                    .AddItem(new MenuItemDefinition("Host_Tenants", new FixedLocalizableString("租户"), url: "Tenants"))
                    .AddItem(new MenuItemDefinition("Host_Roles", new FixedLocalizableString("角色"), url: "Roles"))
                    .AddItem(new MenuItemDefinition("Host_Editions", new FixedLocalizableString("版本及功能"), url: "Editions"))
                    // .AddItem(new MenuItemDefinition("Host_DepotFeatures", new FixedLocalizableString("租户分部功能"), url: "DepotFeatures"))
                    .AddItem(new MenuItemDefinition("Host_AppSettings", new FixedLocalizableString("应用设置"), url: "AppSettings"))

                // Admin
                ).AddItem(new MenuItemDefinition("AdminPages_Setup", new FixedLocalizableString("配置"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Admin_Setup)
                    .AddItem(new MenuItemDefinition("Admin_Settings", new FixedLocalizableString("全局设置"), url: "TenantSettings"))
                    .AddItem(new MenuItemDefinition("Admin_Depots", new FixedLocalizableString("分部"), url: "Depots"))
                    // .AddItem(new MenuItemDefinition("Admin_DepotSettings", new FixedLocalizableString("分部设置"), url: "DepotSettings"))
                    .AddItem(new MenuItemDefinition("Admin_DepotSignins", new FixedLocalizableString("签到时段"), url: "DepotSignins"))
                    .AddItem(new MenuItemDefinition("Admin_TaskTypes", new FixedLocalizableString("任务类型"), url: "TaskTypes"))
                    .AddItem(new MenuItemDefinition("Admin_ArticleTypes", new FixedLocalizableString("物品类型"), url: "ArticleTypes"))
                    .AddItem(new MenuItemDefinition("Admin_RouteTypes", new FixedLocalizableString("线路类型"), url: "RouteTypes"))
                    .AddItem(new MenuItemDefinition("Admin_RouteRoles", new FixedLocalizableString("线路角色"), url: "RouteRoles"))
                    .AddItem(new MenuItemDefinition("Admin_VaultTypes", new FixedLocalizableString("金库操作类型"), url: "VaultTypes"))
                    .AddItem(new MenuItemDefinition("Admin_VaultRoles", new FixedLocalizableString("金库操作角色"), url: "VaultRoles"))
                ).AddItem(new MenuItemDefinition("AdminPages_Data", new FixedLocalizableString("数据"), icon: "fa fa-th-large", requiredPermissionName: PermissionNames.Admin_Data)
                    .AddItem(new MenuItemDefinition("Admin_Warehouses", new FixedLocalizableString("库房"), url: "Warehouses"))
                    .AddItem(new MenuItemDefinition("Admin_Vaults", new FixedLocalizableString("金库"), url: "Vaults"))
                    .AddItem(new MenuItemDefinition("Admin_Workers", new FixedLocalizableString("工作人员"), url: "Workers"))
                    .AddItem(new MenuItemDefinition("Admin_Vehicles", new FixedLocalizableString("车辆"), url: "Vehicles"))
                    .AddItem(new MenuItemDefinition("Admin_Outlets", new FixedLocalizableString("网点"), url: "Outlets"))
                    .AddItem(new MenuItemDefinition("Admin_Outlets", new FixedLocalizableString("物品"), url: "Articles"))
                ).AddItem(new MenuItemDefinition("AdminPages_Manage", new FixedLocalizableString("管理"), icon: "fa fa-list", requiredPermissionName: PermissionNames.Admin_Manage)
                    .AddItem(new MenuItemDefinition("Admin_Users", new FixedLocalizableString("用户"), url: "Users"))
                    .AddItem(new MenuItemDefinition("Admin_Managers", new FixedLocalizableString("公司管理人员"), url: "Managers"))
                    .AddItem(new MenuItemDefinition("Admin_LoginAttempts", new FixedLocalizableString("登录日志"), url: "LoginAttempts"))
                    .AddItem(new MenuItemDefinition("Admin_ErrorLoginAttempts", new FixedLocalizableString("错误登录日志"), url: "ErrorLoginAttempts"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                    .AddItem(new MenuItemDefinition("Dispatcher_DaySettlesQuery", new FixedLocalizableString("日结查询"), url: "DaySettles/DaySettlesQuery"))

                // Dispatcher
                ).AddItem(new MenuItemDefinition("DispatcherPages_Arrange", new FixedLocalizableString("调度"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.DispatcherPages)
                    .AddItem(new MenuItemDefinition("Dispatcher_Signins", new FixedLocalizableString("签到情况"), url: "Signins"))
                    .AddItem(new MenuItemDefinition("Dispatcher_WhAffairs", new FixedLocalizableString("库房任务"), url: "WhAffairs"))
                    .AddItem(new MenuItemDefinition("Dispatcher_VtAffairs", new FixedLocalizableString("金库任务"), url: "VtAffairs"))
                    .AddItem(new MenuItemDefinition("Dispatcher_Routes", new FixedLocalizableString("线路任务"), url: "Routes"))
                ).AddItem(new MenuItemDefinition("DispatcherPages_PreArrange", new FixedLocalizableString("预排"), icon: "fa fa-list", requiredPermissionName: PermissionNames.DispatcherPages)
                    .AddItem(new MenuItemDefinition("Dispatcher_PreRoutes", new FixedLocalizableString("预排线路"), url: "PreRoutes"))
                    .AddItem(new MenuItemDefinition("Dispatcher_PreWorkers", new FixedLocalizableString("预排人员"), url: "PreWorkers"))
                    .AddItem(new MenuItemDefinition("Dispatcher_VehicleWorkers", new FixedLocalizableString("应急交接人员"), url: "VehicleWorkers"))
                ).AddItem(new MenuItemDefinition("DispatcherPages_Report", new FixedLocalizableString("日结统计"), icon: "fa fa-envelope", requiredPermissionName: PermissionNames.DispatcherPages)
                    .AddItem(new MenuItemDefinition("Dispatcher_DaySettle", new FixedLocalizableString("日结"), url: "DaySettles"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteTasksQuery", new FixedLocalizableString("线路任务查询"), url: "Routes/RouteTasksQuery"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteTasksStat", new FixedLocalizableString("线路任务统计"), url: "Routes/RouteTasksStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_SigninsStat", new FixedLocalizableString("签到统计"), url: "Signins/SigninsStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteWorkersStat", new FixedLocalizableString("线路工作量统计"), url: "Routes/RouteWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_VtAffairWorkersStat", new FixedLocalizableString("金库工作量统计"), url: "VtAffairs/VtAffairWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_WhAffairWorkersStat", new FixedLocalizableString("库房工作量统计"), url: "WhAffairs/WhAffairWorkersStat"))

                // SubDispatcher
                ).AddItem(new MenuItemDefinition("AuxDispatcherPages", new FixedLocalizableString("辅助调度"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.AuxDispatcherPages)
                    .AddItem(new MenuItemDefinition("AuxDispatcher_WhAffairsCheck", new FixedLocalizableString("库房人员自签"), url: "Keeper/WhAffairsCheck"))
                    .AddItem(new MenuItemDefinition("AuxDispatcher_RouteTasks", new FixedLocalizableString("线路任务安排"), url: "Routes/TaskArrange"))

                // Keeper
                ).AddItem(new MenuItemDefinition("KeeperPages_Inquire", new FixedLocalizableString("库房查询"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.KeeperPages)
                    .AddItem(new MenuItemDefinition("Keeper_WhAffairsCheck", new FixedLocalizableString("库房人员自签"), url: "Keeper/WhAffairsCheck"))
                    .AddItem(new MenuItemDefinition("Keeper_Articles", new FixedLocalizableString("物品清单"), url: "Keeper/Index"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                ).AddItem(new MenuItemDefinition("KeeperPages_Operate", new FixedLocalizableString("库房操作"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.KeeperPages)
                    .AddItem(new MenuItemDefinition("Keeper_VtAffairsCheck", new FixedLocalizableString("金库人员核实"), url: "Keeper/VtAffairsCheck"))
                    .AddItem(new MenuItemDefinition("Keeper_RoutesArticle", new FixedLocalizableString("押运任务处理"), url: "Keeper/RoutesArticle"))

                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TbsConsts.LocalizationSourceName);
        }
    }
}
