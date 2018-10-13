using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// RouteTask Entity
    /// </summary>
    [Description("线路任务")]
    public class RouteTask : Entity
    {
        public const int MaxRemarkLength = 50;
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// 预计达到时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string ArriveTime { get; set; }

        /// <summary>
        /// 外键：银行网点
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }
        /// <summary>
        /// 外键：任务类型
        /// </summary>
        [Required]
        public int TaskTypeId { get; set; }
        public virtual TaskType TaskType { get; set; }

        /// <summary>
        /// 身份确认时间
        /// </summary>
        public DateTime? IdentifyTime { get; set; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public int? Qtum { get; set; }
        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

