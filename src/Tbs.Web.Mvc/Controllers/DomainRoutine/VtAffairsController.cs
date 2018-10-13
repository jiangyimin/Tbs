﻿using Abp.AspNetCore.Mvc.Authorization;
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
    public class VtAffairsController : TbsControllerBase
    {
        private readonly IVtAffairAppService _vtAffairAppService;

        public VtAffairsController(IVtAffairAppService vtAffairAppService)
        {
            _vtAffairAppService = vtAffairAppService;
        }

        public ActionResult Index()
        {
            int depotId = DomainManager.GetDepotId();
            var vaults = DomainManager.GetVaults(depotId);
            var user = DomainManager.GetCurrentUser();
            var vm = new VtAffairsViewModel() 
            {
                Today = DateTime.Now.ToString("yyyy-MM-dd"),
                DepotId = depotId,
                VtNames = new List<string>(vaults.Select(w => w.Name).ToList()),
                Finger = DomainManager.GetUserWorker().Finger,
                OperatePassword = user.GetOpPassword(),
                PwdDeadline = user.GetOpDeadline().ToString("yyyy-MM-dd HH")
            };
            return View(vm);
        }

        public ActionResult VtAffairWorkersStat()
        {
            VtAffairsViewModel vm = new VtAffairsViewModel() {
                VaultRoles = DomainManager.GetVaultRoles(),
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        [DontWrapResult]
        public JsonResult GridData(int depotId, DateTime carryoutDate)
        {
            var output = _vtAffairAppService.GetAffairs(depotId, carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult WorkersGridData(int id)
        {
            var output = _vtAffairAppService.GetAffairWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

    }
}