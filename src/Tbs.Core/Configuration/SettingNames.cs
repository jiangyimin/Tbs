namespace Tbs.Configuration
{
    public static class SettingNames
    {
        public static class EntityRules
        {
            public const string NormalNameMinLength = "EntityRules.NormalNameMinLength";
            public const string UserNameMinLength = "EntityRules.UserNameMinLength";
            public const string PasswordMinLength = "EntityRules.PasswordMinLength";
            public const string OutletCnLength = "EntityRules.OutletCnLength";
            public const string OutletNameMinLength = "EntityRules.OutletNameMinLength";
            public const string OutletPasswordLength = "EntityRules.OutletPasswordLength";
            public const string VehicleCnLength = "EntityRules.VehicleCnLength";
            public const string WorkerCnLength = "EntityRules.WorkerCnLength";
            public const string WorkerPasswordLength = "EntityRules.WorkerPasswordLength";
            public const string ArticleCnLength = "EntityRules.ArticleCnLength";
        }

        public static class UI
        {
            public const string TopBarHeight = "UI.TopBarHeight";
            public const string CompanyName = "UI.CompanyName";
            public const string CompanyImageName = "UI.CompanyImageName";
            public const string DepotName = "UI.DepotName";
        }

        public static class Weixin
        {
            public const string CorpId = "Weixin.CorpId";
            public const string Secret = "Weixin.Secret";
            public const string AgentId = "Weixin.AgentId";
            public const string Token = "Weixin.Token";
            public const string EncodingAESKey = "Weixin.EncodingAESKey";
            // public const string UseIPAddressSignin = "Weixin.UseIPAddressSignin";
            // public const string IPAddress = "Weixin.IPAddress";
       }

       public static class WorkFlow
       {
           public const string MustSigninOnRouteActive = "WorkFlow.MustSigninOnRouteActive";
           public const string TaskTypeListForRoutesFrom = "WorkFlow.TaskTypeListForRoutesFrom";
           public const string IdentifyNeedSubWorker = "WorkFlow.IdentifyNeedSubWorker";
           public const string DefautPasswordExpiryHours = "WorkFlow.DefautPasswordExpiryHours";
           public const string ManagersForDaySettleNotify = "WorkFlow.ManagersForDaySettleNotify";
           public const string LoginIpList = "WorkFlow.LoginIpList";
       }
    }
}