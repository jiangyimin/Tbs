using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface ISettleAppService : IApplicationService
    {
        Task<PagedResultDto<DaySettleListDto>> GetPagedResult(int depotId, PagedAndSortedResultRequestDto input);

        int CreateOrGet(int depotId, DateTime caryyoutDate);

        bool IsSettled(int depotId, DateTime carryoutDate);

    }

}