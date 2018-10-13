using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// RouteWorker Entity
    /// </summary>
    [Description("线路人员")]
    public class RouteWorker : Entity
    {
        public const int MaxRecordListLength = 50;
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        [Required]
        public int WorkerId { get; set; }
        [Required]
        [StringLength(Worker.MaxCnLength)]
        public string WorkerCn { get; set; }
        [Required]
        [StringLength(Worker.MaxNameLength)]
        public string WorkerName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public int RouteRoleId { get; set; }
        [Required]
        [StringLength(RouteRole.MaxNameLength)]
        public string RouteRoleName { get; set; }

        /// <summary>
        /// 物品领用记录列表
        /// </summary>
        [StringLength(MaxRecordListLength)]
        public string RecordList { get; set; }

        /// <summary>
        /// 签入时间
        /// </summary>
        public DateTime? LendTime { get; set; }

        /// <summary>
        /// 签出时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }
    }
}

