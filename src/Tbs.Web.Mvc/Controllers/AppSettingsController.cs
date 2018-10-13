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
using Tbs.MultiTenancy;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.HostPages)]
    public class AppSettingsController : TbsControllerBase
    {
        private readonly ISettingAppService _settingAppService;

        private readonly TbsCoreSettingProvider _provider;
        public AppSettingsController(ISettingAppService settingAppService, TbsCoreSettingProvider provider) 
        {
            _settingAppService = settingAppService;
            _provider = provider;
        }

        public ActionResult Index()
        {
            var settings = _provider.GetSettingDefinitions(null).Where(sd=> sd.Scopes == SettingScopes.Application);

            return View(settings);
        }

        [DontWrapResult]
        public JsonResult GridData()
        {
            var settings = _settingAppService.GetSettingsForApplication();

            return Json(new {total=settings.Count(), rows=settings}); 
        }
	}
}