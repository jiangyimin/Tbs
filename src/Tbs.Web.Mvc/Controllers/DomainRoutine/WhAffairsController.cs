using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Tbs.Web.Models;
using Tbs.DomainModels;
using Abp.UI;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.DispatcherPages)]
    public class WhAffairsController : TbsControllerBase
    {
        private readonly IWhAffairAppService _whAffairAppService;

        public WhAffairsController(IWhAffairAppService whAffairAppService)
        {
            _whAffairAppService = whAffairAppService;
        }

        public ActionResult Index()
        {
            int depotId = DomainManager.GetDepotId();
            var warehouses = DomainManager.GetWarehouses(depotId);
            var user = DomainManager.GetCurrentUser();
            var vm = new WhAffairsViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                WhNames = new List<string>(warehouses.Select(w => w.Name).ToList()),
                Finger = DomainManager.GetUserWorker().Finger,
                OperatePassword = user.GetOpPassword(),
                PwdDeadline = user.GetOpDeadline().ToString("yyyy-MM-dd HH")
            };
            return View(vm);
        }

        public ActionResult WhAffairWorkersStat()
        {
            WhAffairsViewModel vm = new WhAffairsViewModel() {
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult GridData(int depotId, DateTime carryoutDate)
        {
            var output = _whAffairAppService.GetAffairs(depotId, carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult WorkersGridData(int id)
        {
            var output = _whAffairAppService.GetAffairWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

    }
}