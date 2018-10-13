using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// DepotSetting Entity
    /// </summary>
    [Description("运行中心的设置")]
    public class DepotSetting : Entity
    {
        public const int MaxNameLength = 8;

        public int DepotId { get; set; }

        /// <summary>
        /// 设置名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Value { get; set; }
    }
}

