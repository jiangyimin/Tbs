using Abp.AspNetCore.Mvc.Authorization;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tbs.Controllers;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : TbsControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        public HomeController(IUserNavigationManager userNavigationManager) 
        {
            _userNavigationManager = userNavigationManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetUserMenu()
        {
            var usermenu = await _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier());
            return Json(usermenu.Items);
        }
	}
}