using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Authorization;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Manage)]
    public class ManagersController : TbsCrudController<Manager, ManagerDto>
    {
        public ManagersController(IRepository<Manager> repository)
            : base(repository)
        {
        }
	}
}