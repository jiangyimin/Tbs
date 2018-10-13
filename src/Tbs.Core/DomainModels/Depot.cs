using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Depot Entity
    /// </summary>
    [Description("运行中心")]
    public class Depot : Entity, IMustHaveTenant
    {        
        public const int MaxCnLength = 2;
        public const int MaxNameLength = 8;
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 获取或设置 中心名
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }

        public bool UseRouteForIdentify { get; set; }
    }
}

