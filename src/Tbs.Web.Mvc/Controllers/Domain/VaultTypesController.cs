using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Authorization;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class VaultTypesController : TbsCrudController<VaultType, VaultTypeDto>
    {
        public VaultTypesController(IRepository<VaultType> repository)
            : base(repository)
        {
        }
	}
}