using System.Threading.Tasks;
using Abp.Application.Services;
using Tbs.Authorization.Accounts.Dto;

namespace Tbs.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
