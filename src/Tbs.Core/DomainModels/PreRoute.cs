using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// PreRoute Entity
    /// </summary>
    [Description("预排线路")]
    public class PreRoute : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 8;
        public const int MaxNameLength = 20;
        public const int MaxRemarkLength = 50;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：中心Id
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// RouteType
        /// </summary>
        [Required]
        public int RouteTypeId { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string RouteCn { get; set; }

        /// <summary>
        /// 线路号
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string RouteName { get; set; }

        /// <summary>
        /// 预计出发时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string SetoutTime { get; set; }

        /// <summary>
        /// 预计返回时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string ReturnTime { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        // [Required]
        public int? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        [ForeignKey("PreRouteId")]
        public virtual ICollection<PreRouteTask> Tasks { get; set; }
    }
}

