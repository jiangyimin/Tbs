using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface IPreRouteAppService : IApplicationService
    {
        List<PreRouteDto> GetPreRoutes(int depotId, int routeTypeId, string sorting);
        Task<PreRouteDto> Insert(PreRouteDto input);
        Task<PreRouteDto> Update(PreRouteDto input);
        Task Delete(int id);
        Task Deletes(List<int> ids);

        List<PreRouteTaskDto> GetPreRouteTasks(int preRouteid, string sorting);
        Task<PreRouteTaskDto> InsertSon(PreRouteTaskDto input);
        Task<PreRouteTaskDto> UpdateSon(PreRouteTaskDto input);
        Task DeleteSon(int id);
        Task DeletesSon(List<int> ids);
    }
}