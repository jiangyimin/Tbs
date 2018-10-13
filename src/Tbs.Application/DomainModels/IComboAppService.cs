using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.DomainModels
{
    public interface IComboAppService : IApplicationService
    {
        Task<List<ComboboxItemDto>> GetComboItems(string name, string key, string value, params string[] appdix);
        Task<List<WorkerKVDto>> GetWorkerItems(int depotId);
        Task<List<VehicleKVDto>> GetVehicleItems(int depotId);
        Task<List<OutletKVDto>> GetOutletItems(int depotId);
        Task<List<VaultRoleDto>> GetVaultRoleItems(int vaultTypeId);       
        Task<List<RouteRoleDto>> GetRouteRoleItems(int routeTypeId);       
    }
}
