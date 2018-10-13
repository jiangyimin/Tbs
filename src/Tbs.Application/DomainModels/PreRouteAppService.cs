using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Tbs.DomainModels.Dto;
using System.Collections.Generic;

namespace Tbs.DomainModels
{
    public class PreRouteAppService : TbsAppServiceBase, IPreRouteAppService
    {
        private readonly IRepository<PreRoute> _routeRepository;
        private readonly IRepository<PreRouteTask> _taskRepository;

        public PreRouteAppService(IRepository<PreRoute> routeRepository, IRepository<PreRouteTask> taskRepository)
        {
            _routeRepository = routeRepository;
            _taskRepository = taskRepository;
        }

        public List<PreRouteDto> GetPreRoutes(int depotId, int routeTypeId, string sorting)
        {
            var o = _routeRepository.GetAll().Where($"DepotId = {depotId} && RouteTypeId = {routeTypeId}").OrderBy(sorting).ToList();              
            return new List<PreRouteDto>(o.Select(ObjectMapper.Map<PreRouteDto>).ToList());
        }

        public async Task<PreRouteDto> Insert(PreRouteDto input)
        {
            var entity = ObjectMapper.Map<PreRoute>(input);

            await _routeRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteDto>(entity);
        }

        public async Task<PreRouteDto> Update(PreRouteDto input)
        {
            var entity = _routeRepository.Get(input.Id);
            ObjectMapper.Map<PreRouteDto, PreRoute>(input, entity);
            await _routeRepository.UpdateAsync(entity);
            return ObjectMapper.Map<PreRouteDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _routeRepository.DeleteAsync(id);
        }

        public async Task Deletes(List<int> ids)
        {
            foreach (int id in ids)
                await _routeRepository.DeleteAsync(id);
        }

        #region Task
        public List<PreRouteTaskDto> GetPreRouteTasks(int preRouteId, string sorting)
        {
            var o = _taskRepository.GetAll().Where(e => e.PreRouteId == preRouteId).OrderBy(sorting).ToList();

            return new List<PreRouteTaskDto>(o.Select(ObjectMapper.Map<PreRouteTaskDto>).ToList());
        }

        public async Task<PreRouteTaskDto> UpdateSon(PreRouteTaskDto input)
        {
            var entity = await _taskRepository.GetAsync(input.Id);
            ObjectMapper.Map<PreRouteTaskDto, PreRouteTask>(input, entity);

            await _taskRepository.UpdateAsync(entity);
            return ObjectMapper.Map<PreRouteTaskDto>(entity);
        }

        public async Task<PreRouteTaskDto> InsertSon(PreRouteTaskDto input)
        {
            var entity = ObjectMapper.Map<PreRouteTask>(input);

            await _taskRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteTaskDto>(entity);
        }

        public async Task DeleteSon(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        public async Task DeletesSon(List<int> ids)
        {
            foreach (int id in ids)
                await _taskRepository.DeleteAsync(id);
        }
        #endregion
    }
}