using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using Tbs.Features;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.UI;
using Tbs.Editions;
using Abp.Extensions;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Setup)]
    public class DepotSigninsController : TbsCrudController<DepotSignin, DepotSigninDto>
    {
        public DepotSigninsController(IRepository<DepotSignin> repository)
            :base(repository)
        {
        }
	}
}