using Abp.Authorization;
using Tbs.Authorization.Roles;
using Tbs.Authorization.Users;
using Tbs.MultiTenancy;

namespace Tbs.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
