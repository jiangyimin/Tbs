using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Tbs.MultiTenancy;
using Microsoft.AspNetCore.Mvc;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.HostPages)]
    public class TenantsController : TbsControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}