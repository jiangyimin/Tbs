using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// DepotFeature Entity
    /// </summary>
    [Description("运行中心的功能")]
    public class DepotFeature : Entity
    {
        public const int MaxNameLength = 8;

        public int DepotId { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }
    }
}

