using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Tbs.MultiTenancy;
using Abp.Runtime.Session;
using Abp.IdentityFramework;
using Tbs.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Tbs.DomainServices;

namespace Tbs
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class TbsAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public DomainManager DomainManager { get; set; }

        protected TbsAppServiceBase()
        {
            LocalizationSourceName = TbsConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}