using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Entities;
using Abp.Linq;
using Tbs.DomainModels.Dto;
using System.Collections.Generic;

namespace Tbs.Controllers
{
    public abstract class TbsCrudControllerBase<TEntity, TEntityDto> : TbsCrudControllerBase<TEntity, int, TEntityDto>
        where TEntity : class, IEntity
        where TEntityDto : IInfoEntityDto
    {
        protected TbsCrudControllerBase(IRepository<TEntity, int> repository)
            : base(repository)
        {

        }
    }

    public abstract class TbsCrudControllerBase<TEntity, TPrimaryKey, TEntityDto> : TbsControllerBase
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IInfoEntityDto<TPrimaryKey>
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;

        protected TbsCrudControllerBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            Repository = repository;
        }

        protected async Task<List<TEntityDto>> GetListResult(string wherePhrase)
        {
            var query = wherePhrase == null ? Repository.GetAll() : Repository.GetAll().Where(wherePhrase);
            query = query.OrderBy(GetSorting());                               // Applying Sorting
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new List<TEntityDto>(entities.Select(MapToEntityDto).ToList());
        }
        protected async Task<PagedResultDto<TEntityDto>> GetPagedResult(string wherePhrase)
        {
            var query = wherePhrase == null ? Repository.GetAll() : Repository.GetAll().Where(wherePhrase);
            var input = GetPagedInput();

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);     // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }
        protected async Task<TEntityDto> Create(TEntityDto input)
        {
            var entity = MapToEntity(input);

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected async Task<TEntityDto> Update(TEntityDto input)
        {
            var entity = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected async Task<TEntityDto> Delete(TPrimaryKey id)
        {
            var entity = await GetEntityByIdAsync(id);
            await Repository.DeleteAsync(id);
            return MapToEntityDto(entity);
        }

        #region private methods

        /// <summary>
        /// Maps <see cref="TEntity"/> to <see cref="TEntityDto"/>.
        /// It uses <see cref="IObjectMapper"/> by default.
        /// It can be overrided for custom mapping.
        /// </summary>
        private TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        /// <summary>
        /// Maps <see cref="TEntityDto"/> to <see cref="TEntity"/> to create a new entity.
        /// It uses <see cref="IObjectMapper"/> by default.
        /// It can be overrided for custom mapping.
        /// </summary>
        private TEntity MapToEntity(TEntityDto input)
        {
            return ObjectMapper.Map<TEntity>(input);
        }

        /// <summary>
        /// Maps <see cref="TUpdateInput"/> to <see cref="TEntity"/> to update the entity.
        /// It uses <see cref="IObjectMapper"/> by default.
        /// It can be overrided for custom mapping.
        /// </summary>
        protected virtual void MapToEntity(TEntityDto input, TEntity entity)
        {
            ObjectMapper.Map(input, entity);
        }

        private Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            return Repository.GetAsync(id);
        }
        #endregion
    }
}