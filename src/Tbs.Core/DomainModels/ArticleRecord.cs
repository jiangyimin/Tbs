using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// ArticleRecord Entity
    /// </summary>
    [Description("物品领用记录")]
    public class ArticleRecord : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = 250;
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
        /// <summary>
        /// 外键：中心Id
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; } 

        /// <summary>
        /// 外键：物品
        /// </summary>
        [Required]
        public int ArticleId { get; set; }

        /// <summary>
        /// 外键：领用人
        /// </summary>
        [Required]
        public int WorkerId { get; set; }
        [Required]
        [StringLength(Worker.MaxCnLength)]
        public string WorkerCn { get; set; }
        [Required]
        [StringLength(Worker.MaxNameLength)]
        public string WorkerName { get; set; }

        /// <summary>
        /// 领出时间
        /// </summary>
        [Required]
        public DateTime LendTime { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

