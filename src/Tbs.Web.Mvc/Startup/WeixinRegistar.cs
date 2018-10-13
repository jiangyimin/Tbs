using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Senparc.Weixin.Work.Containers;
using Tbs.Configuration;
using Tbs.DomainServices;
using Tbs.MultiTenancy;

namespace Tbs.Web.Startup
{

    // Not Used , now register in Module

    public class WeixinRegistar
    {        
        // private static readonly string _corpId = "wx9b674b6377fc64be";
        // private static readonly string _secret = "F0drNfu4Qdy6HStvuX6HsZMmcGcj7vc_R6tDvpWaLLw";

        public static void Register()
        {
            List<int> ids = new List<int>() { 1, 2 };
            var SettingManager = IocManager.Instance.Resolve<SettingManager>();
            //var tenants = .OrderBy(t => t.Id).ToList(); //FindByTenancyNameAsync("Default").Result;
            foreach (int id in ids)
            {
                string corpId = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.CorpId, id).Result;
                string secret = SettingManager.GetSettingValueForTenantAsync(SettingNames.Weixin.Secret, id).Result;

                if (!string.IsNullOrEmpty(corpId) && !string.IsNullOrEmpty(secret))
                {
                    AccessTokenContainer.Register(corpId, secret);
                    JsApiTicketContainer.Register(corpId, secret);

                }
           }
        }
    }
}