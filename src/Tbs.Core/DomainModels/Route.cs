using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Route Entity
    /// </summary>
    [Description("线路")]
    public class Route : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }
        /// <summary>
        /// 外键：中心Id
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        [Required]
        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// 状态（安排，活动，结束, 日结）
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Status { get; set; }

        [NotMapped]
        public string ActivateInfo { get; set; }
        
         /// <summary>
        /// 线路类型
        /// </summary>
        [Required]
        public int RouteTypeId { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        [Required]
        [StringLength(PreRoute.MaxCnLength)]
        public string RouteCn { get; set; }

        /// <summary>
        /// 线路号
        /// </summary>
        [Required]
        [StringLength(PreRoute.MaxNameLength)]
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
        /// 外键: 车辆
        /// </summary>
        [Required]
        public int? VehicleId { get; set; }

        /// <summary>
        /// 线路说明
        /// </summary>
        [MaxLength(PreRoute.MaxRemarkLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        [ForeignKey("RouteId")]
        public virtual List<RouteTask> Tasks { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        [ForeignKey("RouteId")]
        public virtual List<RouteWorker> Workers { get; set; }
    }
}

