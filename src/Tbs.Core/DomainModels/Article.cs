using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Article Entity
    /// </summary>
    [Description("物品")]
    public class Article : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 10;
        public const int MaxNameLength = 20;
        public const int MaxRfidLength = 20;
        public const int MaxBindInfoLength = 20;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 物品名
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [Required]
        public int ArticleTypeId { get; set; }
        public virtual ArticleType ArticleType { get; set; }

        /// <summary>
        /// Rfid 
        /// </summary>
        [StringLength(MaxRfidLength)]
        public string Rfid { get; set; }

        /// <summary>
        /// 绑定信息 
        /// </summary>
        [StringLength(MaxBindInfoLength)]
        public string BindInfo { get; set; }

        /// <summary>
        /// 最近进出记录号
        /// </summary>
        public int? ArticleRecordId { get; set; }

        public virtual ArticleRecord ArticleRecord { get; set;}
    }
}

