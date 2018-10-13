using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Authorization;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class RouteTypesController : TbsCrudController<RouteType, RouteTypeDto>
    {
        public RouteTypesController(IRepository<RouteType> repository)
            : base(repository)
        {
        }
	}
}