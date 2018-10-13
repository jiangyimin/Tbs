using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Vault))]
    public class VaultDto : InfoEntityDto
    {
        [Required]
        public int DepotId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 核实库房
        /// </summary>
        public int WarehouseId { get; set;}

        #region Interface implenments
        public override string Info { get { return $"金库名为{Name}"; } }

        #endregion
    }
}