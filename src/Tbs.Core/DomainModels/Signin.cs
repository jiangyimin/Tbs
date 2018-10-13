using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;
using System;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Signin Entity
    /// </summary>
    [Description("签到")]
    public class Signin : Entity, IMustHaveTenant
    {        
        public const int MaxWorkerLength = 20;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// 签到时段名称
        /// </summary>
        [Required]
        [StringLength(DepotSignin.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        [Required]
        [StringLength(MaxWorkerLength)]
        public string Worker { get; set; }
        
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SigninTime { get; set; }

        /// <summary>
        /// 迟到偏离量（- 为提前，按分钟）
        /// </summary>
        public int LateDistance { get; set; }
    }
}

