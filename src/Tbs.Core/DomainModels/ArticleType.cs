using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Article Entity
    /// </summary>
    [Description("物品类型")]
    public class ArticleType : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 1;
        public const int MaxNameLength = 8;

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }
        
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 绑定到（人\车\线）
        /// </summary>
        [StringLength(1)]
        public string BindTo { get; set; }
        
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
    }
}

