using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// RouteType Entity
    /// </summary>
    [Description("线路类型")]
    public class RouteType : Entity, IMustHaveTenant
    {
         public const int MaxNameLength = 8;

        // Implement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 时间段(setout and return)之最早时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string EarliestTime { get; set; }

        /// <summary>
        /// 时间段(setout and return)之最晚时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string LatestTime { get; set; }

        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool NeedApproval { get; set; }
        
        /// <summary>
        /// 激活提前量
        /// </summary>
        public int ActivateAhead { get; set; }

        /// <summary>
        /// 物品提前量
        /// </summary>
        public int ArticleAhead { get; set; }

        /// <summary>
        /// 物品Deadline量
        /// </summary>
        public int ArticleDeadline { get; set; }

        /// <summary>
        /// 设置出发角色名称
        /// </summary>
        [StringLength(RouteRole.MaxNameLength)]
        public string SetoutRoleName { get; set; }

        /// <summary>
        /// 身份确认角色名称
        /// </summary>
        [StringLength(RouteRole.MaxNameLength)]
        public string IdentifyRoleName { get; set; }

        /// <summary>
        /// role definitions for this type.
        /// </summary>
        [ForeignKey("RouteTypeId")]
        public virtual List<RouteRole> Roles { get; set; }

    }
}

