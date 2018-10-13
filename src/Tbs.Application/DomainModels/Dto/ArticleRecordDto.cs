using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(ArticleRecord))]
    public class ArticleRecordDto : EntityDto
    {
         /// <summary>
        /// 外键：中心Id
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        
        /// <summary>
        /// 外键：物品
        /// </summary>
        [Required]
        public int ArticleId { get; set; }

        /// <summary>
        /// 外键：领用人
        /// </summary>
        [Required]
        public int WorkerId { get; set; }
        [Required]
        public string WorkerCn { get; set; }
        [Required]
        public string WorkerName { get; set; }

        /// <summary>
        /// 领出时间
        /// </summary>
        [Required]
        public DateTime LendTime { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }

        public string Remark { get; set; }
    }
}

