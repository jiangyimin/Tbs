using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;
using Tbs.Configuration;

namespace Tbs.Configuration
{
    public class TbsCoreSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            List<SettingDefinition> lst = new List<SettingDefinition>();
            lst.AddRange(GetEntityRulesSettingDefinitions(context));
            lst.AddRange(GetUISettingDefinitions(context));
            lst.AddRange(GetWeixinSettingDefinitions(context));
            lst.AddRange(GetWorkFlowSettingDefinitions(context));
            return lst;
        }

        private List<SettingDefinition> GetEntityRulesSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
                   {
                       new SettingDefinition(
                           SettingNames.EntityRules.NormalNameMinLength,
                           "2",
                           new FixedLocalizableString("常规名称最小长度"),
                           scopes: SettingScopes.Application
                           ),
                       new SettingDefinition(
                           SettingNames.EntityRules.UserNameMinLength,
                           "5",
                           new FixedLocalizableString("用户名最小长度"),
                           scopes: SettingScopes.Application
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.PasswordMinLength,
                           "6",
                           new FixedLocalizableString("登录密码最小长度"),
                           scopes: SettingScopes.Application
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.OutletCnLength,
                           "6",
                           new FixedLocalizableString("网点编号长度"),
                           scopes: SettingScopes.Tenant
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.OutletNameMinLength,
                           "4",
                           new FixedLocalizableString("网点名最小长度"),
                           scopes: SettingScopes.Application
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.OutletPasswordLength,
                           "6",
                           new FixedLocalizableString("网点密码长度"),
                           scopes: SettingScopes.Application
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.VehicleCnLength,
                           "3",
                           new FixedLocalizableString("车辆编号长度"),
                           scopes: SettingScopes.Tenant
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.WorkerCnLength,
                           "5",
                           new FixedLocalizableString("人员编号长度"),
                           scopes: SettingScopes.Tenant
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.WorkerPasswordLength,
                           "6",
                           new FixedLocalizableString("人员密码长度"),
                           scopes: SettingScopes.Application
                           ),
                        new SettingDefinition(
                           SettingNames.EntityRules.ArticleCnLength,
                           "6",
                           new FixedLocalizableString("物品编号长度"),
                           scopes: SettingScopes.Tenant
                           ),
                   };
        }

        private List<SettingDefinition> GetUISettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
                   {
                       new SettingDefinition(
                           SettingNames.UI.TopBarHeight,
                           "45px",
                           new FixedLocalizableString("顶栏高度"),
                           scopes: SettingScopes.Application
                           ),
                       new SettingDefinition(
                           SettingNames.UI.CompanyName,
                           "XX押运",
                           new FixedLocalizableString("公司名"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.UI.CompanyImageName,
                           "logowater.jpg",
                           new FixedLocalizableString("公司图标文件名"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.UI.DepotName,
                           "大队",
                           new FixedLocalizableString("分部俗称"),
                           scopes: SettingScopes.Tenant, 
                           isVisibleToClients: true
                           )
                   };
        }

        private List<SettingDefinition> GetWeixinSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
                   {
                       new SettingDefinition(
                           SettingNames.Weixin.CorpId,
                           "",
                           new FixedLocalizableString("企业Id"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.Weixin.Secret,
                           "",
                           new FixedLocalizableString("管理Secret"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.Weixin.AgentId,
                           "2",
                           new FixedLocalizableString("应用Id"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.Weixin.Token,
                           "",
                           new FixedLocalizableString("应用Token"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.Weixin.EncodingAESKey,
                           "",
                           new FixedLocalizableString("应用AESKey"),
                           scopes: SettingScopes.Tenant
                           ),
                    //    new SettingDefinition(
                    //        SettingNames.Weixin.UseIPAddressSignin,
                    //        "false",
                    //        new FixedLocalizableString("启用IP地址签到"),
                    //        scopes: SettingScopes.Tenant
                    //        ),
                    //    new SettingDefinition(
                    //        SettingNames.Weixin.IPAddress,
                    //        "127.0.0.1",
                    //        new FixedLocalizableString("IP地址"),
                    //        scopes: SettingScopes.Tenant
                    //        ),
                   };
        }

        private List<SettingDefinition> GetWorkFlowSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
                   {
                       new SettingDefinition(
                           SettingNames.WorkFlow.MustSigninOnRouteActive,
                           "true",
                           new FixedLocalizableString("线路激活必须先签到"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.WorkFlow.TaskTypeListForRoutesFrom,
                           "01|02",
                           new FixedLocalizableString("线路复制时的任务类型列表"),
                           scopes: SettingScopes.Tenant,
                           isVisibleToClients:true
                           ),
                       new SettingDefinition(
                           SettingNames.WorkFlow.IdentifyNeedSubWorker,
                           "true",
                           new FixedLocalizableString("身份确认需要辅助人员"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.WorkFlow.DefautPasswordExpiryHours,
                           "8",
                           new FixedLocalizableString("缺省操作密码失效小时数"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.WorkFlow.ManagersForDaySettleNotify,
                           "",
                           new FixedLocalizableString("日结通知领导名单"),
                           scopes: SettingScopes.Tenant
                           ),
                       new SettingDefinition(
                           SettingNames.WorkFlow.LoginIpList,
                           "",
                           new FixedLocalizableString("登录Ip地址列表"),
                           scopes: SettingScopes.Tenant
                           ),
                   };
        }
    }
}
