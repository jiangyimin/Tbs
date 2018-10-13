using Abp.AspNetCore.Mvc.Authorization;
using Tbs.Authorization;
using Tbs.Users.Dto;
using Abp.Domain.Repositories;
using Abp.Authorization.Users;

namespace Tbs.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Admin_Manage)]
    public class LoginAttemptsController : TbsCrudController<UserLoginAttempt, long, LoginAttemptDto>
    {
        public LoginAttemptsController(IRepository<UserLoginAttempt, long> repository)
            :base(repository)
        {
        }
	}
}