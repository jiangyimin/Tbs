using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Tbs.Web.Models;
using Tbs.DomainModels;
using Abp.UI;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.DispatcherPages)]
    public class PreRoutesController : TbsControllerBase
    {
        private readonly IPreRouteAppService _preRouteAppService;
        private readonly IComboAppService _comboAppService;

        public PreRoutesController(IPreRouteAppService preRouteAppService, IComboAppService comboAppService)
        {
            _preRouteAppService = preRouteAppService;
            _comboAppService = comboAppService;
        }

        public ActionResult Index()
        {
            var types = _comboAppService.GetComboItems("RouteType", "Id", "Name").Result;
            if (types.Count == 0)
                throw new UserFriendlyException("请先设置线路类型");

            int depotId = DomainManager.GetDepotId();
            var user = DomainManager.GetCurrentUser();
            var vm = new PreRoutesViewModel() 
            {
                DepotId = depotId,
                RouteTypes = types,
                Finger = DomainManager.GetUserWorker().Finger,
                OperatePassword = user.GetOpPassword(),
                PwdDeadline = user.GetOpDeadline().ToString("yyyy-MM-dd HH")
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult GridData(int depotId, int routeTypeId)
        {
            var output = _preRouteAppService.GetPreRoutes(depotId, routeTypeId, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult RouteTaskGridData(int id)
        {
            var output = _preRouteAppService.GetPreRouteTasks(id, GetSorting());
            return Json( new { rows = output });
        }

    }
}