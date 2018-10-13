using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Tbs.Roles;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Tbs.MultiTenancy;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.HostPages)]
    public class RolesController : TbsControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IRoleAppService _roleAppService;

        public RolesController(ITenantAppService tenantAppService, IRoleAppService roleAppService)
        {
            _tenantAppService = tenantAppService;
            _roleAppService = roleAppService;
        }

        public ActionResult Index()
        {
            var permissions = PermissionManager.GetAllPermissions(false);            
            return View(permissions);
        }

       [DontWrapResult]
        public JsonResult GridData()
        {
            var output = _tenantAppService.GetTenants();
            return Json( new { rows = output.Items });
        }

        [DontWrapResult]
        public JsonResult GetRoles(string id)       // id is tenancyName
        {
            var output = _roleAppService.GetRoles(id);
            return Json( new { rows = output.Items });
        }
    }
}