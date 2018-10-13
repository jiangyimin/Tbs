using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Vault Entity
    /// </summary>
    [Description("金库")]
    public class Vault : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        /// <summary>
        /// 金库名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 核实库房
        /// </summary>
        public int WarehouseId { get; set;}
    }
}

