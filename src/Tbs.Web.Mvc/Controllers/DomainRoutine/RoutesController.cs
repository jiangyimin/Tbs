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
    [AbpMvcAuthorize(PermissionNames.DispatcherPages, PermissionNames.AuxDispatcherPages)]
    public class RoutesController : TbsControllerBase
    {
        private readonly IRouteAppService _routeAppService;

        public RoutesController(IRouteAppService routeAppService)
        {
            _routeAppService = routeAppService;
        }

        public ActionResult Index()
        {
            int depotId = DomainManager.GetDepotId();
            var vaults = DomainManager.GetVaults(depotId);
            var user = DomainManager.GetCurrentUser();
            var vm = new RoutesViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                Finger = DomainManager.GetUserWorker().Finger,
                OperatePassword = user.GetOpPassword(),
                PwdDeadline = user.GetOpDeadline().ToString("yyyy-MM-dd HH")
            };
            return View(vm);
        }

        public ActionResult RouteWorkersStat()
        {
            RoutesViewModel vm = new RoutesViewModel() {
                RouteRoles = DomainManager.GetRouteRoles(),
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult GridData(int depotId, DateTime carryoutDate)
        {
            var output = _routeAppService.GetRoutes(depotId, carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult GridDataActive(int depotId, DateTime carryoutDate)
        {
            var output = _routeAppService.GetRoutesActive(depotId, carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult IdentifiesGridData(int id)
        {
            var output = _routeAppService.GetRouteIdentifies(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult WorkersGridData(int id)
        {
            var output = _routeAppService.GetRouteWorkers(id, GetSorting());
            foreach (var o in output) {
                o.RecordList = DomainManager.GetRecordListDetail(o.RecordList);
            }
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult TasksGridData(int id)
        {
            var output = _routeAppService.GetRouteTasks(id, GetSorting());
            return Json( new { rows = output });
        }

        #region AuxDispatcher
        public ActionResult TaskArrange()
        {
            int depotId = DomainManager.GetDepotId();
            Warehouse myWh = CheckWhAffair(depotId);
            var depots = GetDepotsManaged(depotId, myWh);
            if (depots.Count > 0)
                depotId = depots[0].Key;
            var vm = new RoutesViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                Finger = DomainManager.GetUserWorker().Finger,
                OperatePassword = DomainManager.GetCurrentUser().GetOpPassword()
            };
            return View(vm);
        }

        private Warehouse CheckWhAffair(int depotId)
        {
            var user = DomainManager.GetCurrentUser();
            Warehouse wh = DomainManager.GetWarehouses(depotId).FirstOrDefault(w=>w.Name == user.WhName);
            if (string.IsNullOrEmpty(user.WhName) || wh == null)
                 throw new UserFriendlyException("请系统管理员设置用户所在正确的库房名称");

            string keepers = DomainManager.GetKeepersInfo(depotId, DateTime.Today, user.WhName);
            if (string.IsNullOrEmpty(keepers))
                 throw new UserFriendlyException("请调度安排并激活有对应时段的库房任务");

            if (!keepers.Contains(user.UserName))
                 throw new UserFriendlyException("登录用户未被安排为库房操作人员");
                
            return wh;
        }
        private List<KeyValuePair<int, string>> GetDepotsManaged(int depotId, Warehouse wh)
        {            
            List<KeyValuePair<int, string>> lst = new List<KeyValuePair<int, string>>();

            if (string.IsNullOrEmpty(wh.DepotList))
                return lst;
            foreach (string cn in wh.DepotList.Split('|'))
            {
                Depot depot = DomainManager.GetDepotByCn(cn.Trim());
                if (depot != null)
                    lst.Add(new KeyValuePair<int, string>(depot.Id, depot.Name));
            }
            return lst;
        }

        #endregion
        
    }
}