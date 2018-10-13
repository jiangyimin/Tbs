using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// RouteTask Entity
    /// </summary>
    [Description("线路操作日志")]
    public class RouteLog : Entity
    {
        public const int MaxActionNameLength = 8;
        public const int MaxMessageLength = 200;
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// 操作的userId
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Required]
        public DateTime OperateTime { get; set; }

        [Required]
        [StringLength(MaxActionNameLength)]
        public string ActionName { get; set; }

        [StringLength(MaxMessageLength)]
        public string Message { get; set; }


    }
}

