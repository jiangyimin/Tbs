using System.Threading.Tasks;
using Abp.Application.Services;
using Tbs.Sessions.Dto;

namespace Tbs.Sessions
{
    public interface ISessionAppService : IApplicationService
    {        
        int GetTenantId(string name);

        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
