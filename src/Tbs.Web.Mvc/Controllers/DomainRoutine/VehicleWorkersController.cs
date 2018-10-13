using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Controllers;
using Microsoft.AspNetCore.Mvc;
using Abp.Web.Models;
using Tbs.Web.Models;
using Tbs.DomainModels;
using Abp.UI;
using Tbs.DomainModels.Dto;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.DispatcherPages)]
    public class VehicleWorkersController : TbsCrudController<Vehicle, VehicleWorkerDto>
    {
        public VehicleWorkersController(IRepository<Vehicle> repository)
            : base(repository)
        {
        }

        public override ActionResult Index()
        {
            return View(DomainManager.GetDepotId());
        }
    }
}