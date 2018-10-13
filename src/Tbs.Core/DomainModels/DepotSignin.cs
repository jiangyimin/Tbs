using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Depot Signin Entity
    /// </summary>
    [Description("签到时段")]
    public class DepotSignin : Entity, IMustHaveTenant
    {        
        public const int MaxNameLength = 8;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        /// <summary>
        /// 设置名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Start
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string StartTime { get; set; }

        /// <summary>
        /// End
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string EndTime { get; set; }

        /// <summary>
        /// LateTime
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string LateTime { get; set; }
    }
}

