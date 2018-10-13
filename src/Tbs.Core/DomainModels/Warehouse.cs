using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Warehouse Entity
    /// </summary>
    [Description("库房")]
    public class Warehouse : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;
        public const int ArticleTypeListLength = 50;
        public const int ShiftListLength = 50;
        public const int DepotListLength = 100;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 库房名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 班次列表
        /// </summary>
        [StringLength(ShiftListLength)]
        public string ShiftList { get; set; }

        /// <summary>
        /// 托管分部列表
        /// </summary>
        [StringLength(DepotListLength)]
        public string DepotList { get; set; }     
    }
}

