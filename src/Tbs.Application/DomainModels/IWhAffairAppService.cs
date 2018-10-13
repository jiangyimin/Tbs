using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface IWhAffairAppService : IApplicationService
    {
        List<WhAffairDto> GetAffairs(int depotId, DateTime carryoutDate, string sorting);
        List<WhAffairDto> GetAffairsActive(int depotId, DateTime carryoutDate, string whName, string sorting);
        Task<WhAffairDto> Insert(WhAffairDto input);
        Task<WhAffairDto> Update(WhAffairDto input);
        Task Delete(int id);

        List<WhAffairWorkerDto> GetAffairWorkers(int id, string sorting);
        Task<WhAffairWorkerDto> InsertSon(WhAffairWorkerDto input);
        Task<WhAffairWorkerDto> UpdateSon(WhAffairWorkerDto input);
        Task DeleteSon(int id);

        Task<int> Activate(int depotId, DateTime carryoutDate, string status);
        Task<int> CreateFrom(int depotId, DateTime carryoutDate, DateTime fromDate);
        KeyValuePair<string, string> MatchWorker(Worker worker, int affairId);
        KeyValuePair<string, string> CheckWorker(Worker worker, int affairId);

        string DaySettle(int depotId, DateTime carryoutDate, int settleId);
        Task<List<AffairWorkerStatDto>> Stat(int depotId, DateTime begin, DateTime end);
    }
}