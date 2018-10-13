using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Abp.Domain.Repositories;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Data)]
    public class OutletsController : TbsCrudController<Outlet, OutletDto>
    {
        public OutletsController(IRepository<Outlet> repository)
            :base(repository)
        {
        }
	}
}