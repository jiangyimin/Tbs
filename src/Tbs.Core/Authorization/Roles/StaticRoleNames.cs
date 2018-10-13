namespace Tbs.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";
            public const string Dispatcher = "Dispatcher";
            public const string AuxDispatcher = "AuxDispatcher";
            public const string Keeper = "Keeper";
        }
    }
}