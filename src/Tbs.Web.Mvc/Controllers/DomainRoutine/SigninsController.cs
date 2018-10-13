using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using System;
using Tbs.Web.Models;
using Abp.UI;
using Tbs.DomainModels;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.DispatcherPages)]
    public class SigninsController : TbsControllerBase
    {
        private readonly ISigninAppService _signinAppService;

        public SigninsController(ISigninAppService signinAppService)
        {
            _signinAppService = signinAppService;
        }

        public ActionResult Index()
        {
            SigninsViewModel vm = new SigninsViewModel() {
                DepotId = DomainManager.GetDepotId(),
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                Names = _signinAppService.GetDepotSignins(DomainManager.GetDepotId())
            };

            if (vm.Names.Count == 0)
                throw new UserFriendlyException("请先设置签到班次!");
                
            return View(vm);
        }

        public ActionResult SigninsStat()
        {
            SigninsViewModel vm = new SigninsViewModel() {
                Names = _signinAppService.GetDepotSignins(DomainManager.GetDepotId()),
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult GridData(int depotId, DateTime carryoutDate, string name)
        {
            var output = _signinAppService.GetSignins(depotId, carryoutDate, name);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult AgentGridData(int id)
        {
            var output = _signinAppService.GetSignins(id, "代理");
            return Json( new { rows = output });
        }
    }
}