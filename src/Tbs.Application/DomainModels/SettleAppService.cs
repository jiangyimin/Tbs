using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Tbs.Authorization;
using Abp.Domain.Repositories;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;
using System;
using Abp.Linq;
using System.Threading.Tasks;

namespace Tbs.DomainModels
{
    public class SettleAppService : TbsAppServiceBase, ISettleAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<DaySettle> _settleRepository;
        private readonly IRepository<Signin> _signinRepository;
        public SettleAppService(IRepository<DaySettle> settleRepository, IRepository<Signin> signinRepository)
        {
            _settleRepository = settleRepository;
            _signinRepository = signinRepository;
        }

        public async Task<PagedResultDto<DaySettleListDto>> GetPagedResult(int depotId, PagedAndSortedResultRequestDto input)
        {
            var query = _settleRepository.GetAll().Where(s => s.DepotId == depotId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);     // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = entities.Select(ObjectMapper.Map<DaySettleListDto>).ToList();
            foreach (DaySettleListDto dto in dtos)
            {
                dto.Agent = GetAgentInfo(dto.DepotId, dto.CarryoutDate);
            }

            return new PagedResultDto<DaySettleListDto>(
                totalCount,
                dtos
            );
        }

        public int CreateOrGet(int depotId, DateTime carryoutDate)
        {
            var settle = _settleRepository.FirstOrDefault(s => s.DepotId == depotId && s.CarryoutDate == carryoutDate);
            if (settle == null) {
                settle = new DaySettle() {
                    DepotId = depotId,
                    CarryoutDate = carryoutDate,
                    OperateTime = DateTime.Now
                };

                _settleRepository.Insert(settle);
                CurrentUnitOfWork.SaveChanges();
                
                return settle.Id;
            }
            else {
                return settle.Id;
            }
        }

        public bool IsSettled(int depotId, DateTime carryoutDate)
        {
            var settle = _settleRepository.GetAll().FirstOrDefault(s => s.DepotId == depotId && s.CarryoutDate == carryoutDate);
            
            return settle == null ? false : true;
        }


        #region util
        private string GetAgentInfo(int depotId, DateTime carryoutDate)
        {
            var entity = _signinRepository.FirstOrDefault(s => s.DepotId == depotId && s.CarryoutDate == carryoutDate && s.Name == "代理");
            return entity != null ? entity.Worker : null;
        }
        #endregion

    }
}