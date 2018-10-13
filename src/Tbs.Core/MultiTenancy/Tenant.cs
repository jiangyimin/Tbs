using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;
using Tbs.Authorization.Users;

namespace Tbs.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}