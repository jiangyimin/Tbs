using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.UI;
using Abp.Web.Models;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tbs.DomainModels.Dto;
using Tbs.Controllers;

namespace Tbs.Web.Controllers
{
    public abstract class TbsCrudController<TEntity, TEntityDto> : TbsCrudController<TEntity, int, TEntityDto>
        where TEntity : class, IEntity
        where TEntityDto : IInfoEntityDto
    {
        protected TbsCrudController(IRepository<TEntity> repository)
            : base(repository)
        {
        }
    }

    public abstract class TbsCrudController<TEntity, TPrimaryKey, TEntityDto> : TbsCrudControllerBase<TEntity, TPrimaryKey, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IInfoEntityDto<TPrimaryKey>
    {
        protected TbsCrudController(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public virtual async Task<JsonResult> GridPagedData(string id)          // id is wherePhrase
        {
            try
            {
                PagedResultDto<TEntityDto> o = await GetPagedResult(id);
                return Json(new { total = o.TotalCount, rows = o.Items });
            }
            catch (Exception ex) 
            {
                throw new UserFriendlyException(L("CrudFailed"), ex.Message);
            }
        }

        [DontWrapResult]
        public virtual async Task<JsonResult> GridData(string id)          // id is wherePhrase
        {
            try
            {
                List<TEntityDto> o = await GetListResult(id);
                return Json(new { rows = o });
            }
            catch (Exception ex) 
            {
                throw new UserFriendlyException(L("CrudFailed"), ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> CreateEntity(TEntityDto input)
        {
            try
            {
                var output = await Create(input);
                return Json(new { result = "success", content = $"{output.Info}的记录添加成功" });
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("CrudFailed"), ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> UpdateEntity(TEntityDto input)
        {
            try
            {
                var output = await Update(input);
                return Json(new { result = "success", content = $"{output.Info}的记录修改成功" });
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("CrudFailed"), ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteEntity(TPrimaryKey id)
        {
            try
            {
                var output = await Delete(id);
                return Json(new { result = "success", content = $"{output.Info}的记录删除成功" });
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("CrudFailed"), ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteEntities(List<TPrimaryKey> ids)
        {
            try
            {
                foreach (TPrimaryKey id in ids) 
                {
                    await Delete(id);
                }
                return Json(new { result = "success", content = $"{ids.Count}条记录删除成功" });
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("CrudFailed"), ex.Message);
            }
        }

        protected TEntity Get(TPrimaryKey id)
        {
            return Repository.Get(id);
        }
    }
}