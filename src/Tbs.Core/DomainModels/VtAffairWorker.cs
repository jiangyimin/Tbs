using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// VtAffairWorker Entity
    /// </summary>
    [Description("金库内务工作人员")]
    public class VtAffairWorker : Entity
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int VtAffairId { get; set; }

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
        public int VaultRoleId {get; set; }
        [Required]
        [StringLength(VaultRole.MaxNameLength)]
        public string VaultRoleName { get; set; }

        /// <summary>
        /// 签入时间
        /// </summary>
        public DateTime? CheckIn { get; set; }

        /// <summary>
        /// 签出时间
        /// </summary>
        public DateTime? CheckOut { get; set; }
    }
}

