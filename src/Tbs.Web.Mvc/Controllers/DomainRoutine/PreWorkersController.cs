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
    public class PreWorkersController : TbsCrudController<Vehicle, PreWorkerDto>
    {
        private readonly IComboAppService _comboAppService;

        public PreWorkersController(IRepository<Vehicle> repository, IComboAppService comboAppService)
            : base(repository)
        {
            _comboAppService = comboAppService;
        }

        public override ActionResult Index()
        {
            int depotId = DomainManager.GetDepotId();
            var types = _comboAppService.GetComboItems("RouteType", "Id", "Name").Result;
            if (types.Count == 0 )
                throw new UserFriendlyException("请先设置线路类型");
            var roles = _comboAppService.GetRouteRoleItems(int.Parse(types[0].Value)).Result;
            if (roles.Count > 6)
                throw new UserFriendlyException("线路角色不能多于6个");

            PreWorkersViewModel vm = new PreWorkersViewModel()
            {
                DepotId = depotId,
                RouteRoles = roles
            };
            return View(vm);
        }
    }
}