using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface ISigninAppService : IApplicationService
    {
        List<DepotSigninDto> GetDepotSignins(int depotId);

        string GetAgentInfo(int depotId, DateTime CarryoutDate);

        List<SigninListDto> GetSignins(int depotId, DateTime CarryoutDate, string name);
        List<SigninListDto> GetSignins(int depotId, string name);

        string SigninByIdCardNo(int depotId, string idCardNo, bool agent);

        Task<List<SigninStatDto>> Stat(int depotId, DateTime begin, DateTime end);
    }
}
