using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(VaultType))]
    public class VaultTypeDto : InfoEntityDto
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 时间段(setout and return)之最早时间
        /// </summary>
        [Required]
        public string EarliestTime { get; set; }

        /// <summary>
        /// 时间段(setout and return)之最晚时间
        /// </summary>
        [Required]
        public string LatestTime { get; set; }

        /// <summary>
        /// 是否需要审批
        /// </summary>
        public string NeedApproval { get; set; }
        /// <summary>
        /// 激活提前量
        /// </summary>
        public int ActivateAhead { get; set; }

        /// <summary>
        /// 物品提前量
        /// </summary>
        public int VerifyAhead { get; set; }

        /// <summary>
        /// 物品Deadline量
        /// </summary>
        public int VerifyDeadline { get; set; }

        public List<VaultRoleDto> Roles { get; set; }

        #region Interface implenments
        public override string Info { get { return $"类型为{Name}"; } }
        #endregion
    }
}