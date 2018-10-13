using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Reflection.Extensions;
using Tbs.DomainModels.Dto;
using Tbs.DomainServices;

namespace Tbs.DomainModels
{
    [AbpAuthorize]
    public class ComboAppService : TbsAppServiceBase, IComboAppService
    {
        private readonly IIocResolver _resolver;
        private readonly IRepository<VaultRole> _vaultRoleRepository;
        private readonly IRepository<RouteRole> _routeRoleRepository;

        public ComboAppService(IIocResolver resolver, 
                    IRepository<VaultRole> vaultRoleRepository, IRepository<RouteRole> routeRoleRepository)
        {
            _resolver = resolver;
            _vaultRoleRepository = vaultRoleRepository;
            _routeRoleRepository = routeRoleRepository;
        }

        public Task<List<ComboboxItemDto>> GetComboItems(string name, string key, string value, params string[] appdix)
        {
            Assembly assembly = typeof(TbsCoreModule).GetAssembly();
            Type type = assembly.GetType("Tbs.DomainModels." + name);    
            Type repositoryType = typeof(IRepository<,>).MakeGenericType(type, typeof(int));

            var repository = _resolver.Resolve(repositoryType); 
            MethodInfo method = repositoryType.GetMethod("GetAll", BindingFlags.Instance | BindingFlags.Public);
      
            // get IQueryable<T>
            var query = (IQueryable)method.Invoke(repository, null);
            List<ComboboxItemDto> lst = new List<ComboboxItemDto>();
            foreach (var item in query)
            {
                string keyValue = type.GetProperty(key).GetValue(item).ToString();
                string displayText = type.GetProperty(value).GetValue(item).ToString();
                foreach (string fn in appdix)
                {
                    string appdixText = type.GetProperty(fn).GetValue(item).ToString();
                    displayText += " " + appdixText;
                }
                lst.Add(new ComboboxItemDto(keyValue, displayText));
            }
  
            return Task.FromResult<List<ComboboxItemDto>>(lst);
        }
        public Task<List<VehicleKVDto>> GetVehicleItems(int depotId)
        {
            var vehicles = DomainManager.GetVehicles(depotId).OrderBy(v => v.Cn);
            var lst = new List<VehicleKVDto>(
                vehicles.Select(ObjectMapper.Map<VehicleKVDto>).ToList()
                );
            return Task.FromResult<List<VehicleKVDto>>(lst);
        }

        public Task<List<WorkerKVDto>> GetWorkerItems(int depotId)
        {
            var workers = DomainManager.GetWorkers(depotId).OrderBy(w => w.Cn);
            var lst = new List<WorkerKVDto>(
                workers.Select(ObjectMapper.Map<WorkerKVDto>).ToList()
                );
            return Task.FromResult<List<WorkerKVDto>>(lst);
        }
        public Task<List<OutletKVDto>> GetOutletItems(int depotId)
        {
            var outlets = DomainManager.GetOutlets(depotId).OrderBy(v => v.Cn);
            var lst = new List<OutletKVDto>(
                outlets.Select(ObjectMapper.Map<OutletKVDto>).ToList()
                );
            return Task.FromResult<List<OutletKVDto>>(lst);
        }

        public async Task<List<VaultRoleDto>> GetVaultRoleItems(int vaultTypeId)
        {
            var roles = await _vaultRoleRepository.GetAllListAsync(e => e.VaultTypeId == vaultTypeId);
            var lst = new List<VaultRoleDto>(
                roles.Select(ObjectMapper.Map<VaultRoleDto>).ToList()
                );
            return lst;
        }  
        public async Task<List<RouteRoleDto>> GetRouteRoleItems(int routeTypeId)
        {
            var roles = await _routeRoleRepository.GetAllListAsync(e => e.RouteTypeId == routeTypeId);
            var lst = new List<RouteRoleDto>(
                roles.Select(ObjectMapper.Map<RouteRoleDto>).ToList()
                );
            return lst;
        }  
    }
}