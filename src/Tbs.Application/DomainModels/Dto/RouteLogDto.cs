using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(RouteLog))]
    public class RouteLogDto : EntityDto
    {
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
        public string ActionName { get; set; }

        public string Message { get; set; }
    }
}

