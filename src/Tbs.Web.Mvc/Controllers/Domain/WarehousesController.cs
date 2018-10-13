using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Authorization;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class WarehousesController : TbsCrudController<Warehouse, WarehouseDto>
    {
        public WarehousesController(IRepository<Warehouse> repository)
            : base(repository)
        {
        }
	}
}