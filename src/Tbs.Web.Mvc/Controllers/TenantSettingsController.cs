using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Tbs.Authorization;
using Tbs.Configuration;
using Tbs.Controllers;
using Tbs.Settings;


namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class TenantSettingsController : TbsControllerBase
    {
        private readonly ISettingAppService _settingAppService;
        private readonly TbsCoreSettingProvider _provider;   
        public TenantSettingsController(ISettingAppService settingAppService, TbsCoreSettingProvider provider) 
        {
            _settingAppService = settingAppService;
            _provider = provider;
        }

        public ActionResult Index()
        {
            var settings = _provider.GetSettingDefinitions(null).Where(sd => sd.Scopes.HasFlag(SettingScopes.Tenant));

            return View(settings);
        }

        [DontWrapResult]
        public JsonResult GridData()
        {
            var settings = _settingAppService.GetSettingsForTenant();
            return Json(new {total=settings.Count(), rows=settings}); 
        }
	}
}