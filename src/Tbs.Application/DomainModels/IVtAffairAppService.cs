using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface IVtAffairAppService : IApplicationService
    {
        List<VtAffairDto> GetAffairs(int depotId, DateTime carryoutDate, string sorting);
        List<VtAffairDto> GetAffairsActive(int depotId, DateTime carryoutDate, string sorting);
        
        Task<VtAffairDto> Insert(VtAffairDto input);
        Task<VtAffairDto> Update(VtAffairDto input);
        Task Delete(int id);

        List<VtAffairWorkerDto> GetAffairWorkers(int id, string sorting);
        Task<VtAffairWorkerDto> InsertSon(VtAffairWorkerDto input);
        Task<VtAffairWorkerDto> UpdateSon(VtAffairWorkerDto input);
        Task DeleteSon(int id);

        Task<int> Activate(int depotId, DateTime carryoutDate, string status);
        Task<int> ActivateSelects(List<int> ids, string status);
        
        Task<int> CreateFrom(int depotId, DateTime carryoutDate, DateTime fromDate);

        string GetCheckStatus(int affairId);

        KeyValuePair<string, string> CheckWorker(Worker worker, int affairId);

        string DaySettle(int depotId, DateTime carryoutDate, int settleId);
        Task<List<AffairWorkerStatDto>> Stat(int depotId, DateTime begin, DateTime end);
    }
}