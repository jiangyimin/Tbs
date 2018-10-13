using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Users.Dto;
using Abp.Domain.Repositories;
using Abp.Authorization.Users;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Manage)]
    public class ErrorLoginAttemptsController : TbsCrudController<UserLoginAttempt, long, LoginAttemptDto>
    {
        public ErrorLoginAttemptsController(IRepository<UserLoginAttempt, long> repository)
            :base(repository)
        {
        }
	}
}