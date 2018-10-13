using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Tbs.Users;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Manage)]
    public class UsersController : TbsControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

       [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var output = await _userAppService.GetUsers();
            return Json( new { rows = output.Items });
        }
    }
}