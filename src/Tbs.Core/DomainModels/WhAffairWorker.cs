using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// WhAffairWorker Entity
    /// </summary>
    [Description("库房内务工作人员")]
    public class WhAffairWorker : Entity
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int WhAffairId { get; set; }

        [Required]
        public int WorkerId { get; set; }
        [Required]
        [StringLength(Worker.MaxCnLength)]
        public string WorkerCn { get; set; }
        [Required]
        [StringLength(Worker.MaxNameLength)]
        public string WorkerName { get; set; }

        
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

