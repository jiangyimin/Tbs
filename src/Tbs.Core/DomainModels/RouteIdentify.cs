using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// RouteTask Entity
    /// </summary>
    [Description("线路身份确认")]
    public class RouteIdentify : Entity
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// 外键：银行网点
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }

        /// <summary>
        /// 身份确认时间
        /// </summary>
        public DateTime IdentifyTime { get; set; }
    }
}

