using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Extensions;

namespace Tbs.DomainModels
{
    /// <summary>
    /// RouteRole Entity
    /// </summary>
    [Description("线路角色")]
    public class RouteRole : Entity
    {
        public const int MaxNameLength = 8;
        public const int ArticleTypeListMaxLength = 50;

        /// <summary>
        /// Foreign Id of the RouteType.
        /// </summary>
        public int RouteTypeId { get; set; }
       
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
        /// 可领用物品列表(类型 MAX)，比如(枪 1)(弹 2)
        /// </summary>
        [StringLength(ArticleTypeListMaxLength)]
        public string ArticleTypeList { get; set; }
        
        [NotMapped]
        public bool NeedArticle { get { return !ArticleTypeList.IsNullOrEmpty(); } }
        
        /// <summary>
        /// 同组号
        /// </summary>
        public string PeerGroupNo { get; set; }
    }
}

