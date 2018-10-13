using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// WhAffair Entity
    /// </summary>
    [Description("库房内务")]
    public class WhAffair : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = 50;
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：中心Id
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        [Required]
        public DateTime CarryoutDate { get; set; }
        /// <summary>
        /// 线路号
        /// </summary>
        [Required]
        [StringLength(Warehouse.MaxNameLength)]
        public string WhName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string EndTime { get; set; }

        /// <summary>
        /// 状态（安排，活动，结束, 日结）
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Status { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime LastActiveTime { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }
        /// <summary>
        /// 人员子表
        /// </summary>
        [ForeignKey("WhAffairId")]
        public virtual List<WhAffairWorker> Workers { get; set; }
    }
}

