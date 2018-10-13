using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Extensions;

namespace Tbs.DomainModels
{
    /// <summary>
    /// VaultRole Entity
    /// </summary>
    [Description("金库人员角色")]
    public class VaultRole : Entity
    {
        public const int MaxNameLength = 8;
        public const int ArticleTypeListLength = 50;

        /// <summary>
        /// Foreign Id of the VaultType.
        /// </summary>
        public int VaultTypeId { get; set; }
       
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 是否单个
        /// </summary>
        public bool Single { get; set; }
    }
}

