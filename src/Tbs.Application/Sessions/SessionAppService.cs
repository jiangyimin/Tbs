using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Tbs.MultiTenancy;
using Tbs.Sessions.Dto;
using Tbs.SignalR;

namespace Tbs.Sessions
{
    public class SessionAppService : TbsAppServiceBase, ISessionAppService
    {
        private readonly TenantManager _tenantManager;
        public SessionAppService(TenantManager tenantManager)
        {
            _tenantManager = tenantManager;
        }

        [DisableAuditing]
        public int GetTenantId(string name)
        {
            if (name == null) return 0;
            var tenant = _tenantManager.Tenants.FirstOrDefault(e=>e.Name == name.Trim());
            if (tenant == null) 
                return 0;
            else 
                return tenant.Id;
        }
        
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>
                    {
                        { "SignalR", SignalRFeature.IsAvailable }
                    }
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }
    }
}