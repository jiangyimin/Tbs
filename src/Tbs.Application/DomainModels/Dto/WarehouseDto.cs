using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Warehouse))]
    public class WarehouseDto : InfoEntityDto
    {
        [Required]
        public int DepotId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 可领物品类型列表
        /// </summary>
        public string ArticleTypeList { get; set;}

        /// <summary>
        /// 库房班次列表
        /// </summary>
        public string ShiftList { get; set;}

        /// <summary>
        /// 托管分部列表
        /// </summary>
        public string DepotList { get; set; }     

        #region Interface implenments
        public override string Info { get { return $"库房名为{Name}"; } }

        #endregion
    }
}