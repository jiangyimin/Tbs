using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// DaySettle Entity
    /// </summary>
    [Description("日结")]
    public class DaySettle : Entity, IMustHaveTenant
    {
        public const int MaxMessageLength = 200;
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
        /// 金库任务数量
        /// </summary>
        public int? VtAffairsCount { get; set; }

        /// <summary>
        /// 线路数量
        /// </summary>
        public int? RoutesCount { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }


        [StringLength(MaxMessageLength)]
        public string Message { get; set; }
    }
}

