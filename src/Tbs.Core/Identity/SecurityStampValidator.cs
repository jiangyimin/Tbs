using Abp.Authorization;
using Tbs.Authorization.Roles;
using Tbs.Authorization.Users;
using Tbs.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Tbs.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<IdentityOptions> options, 
            SignInManager signInManager) 
            : base(options, signInManager)
        {
        }
    }
}