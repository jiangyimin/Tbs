using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Validation;
using System;

namespace Tbs.DomainModels.Dto
{
    [AutoMapFrom(typeof(DaySettle))]
    public class DaySettleListDto : EntityDto
    {
        public int DepotId { get; set; }
        
        public DateTime CarryoutDate { get; set; }
        public string Agent { get; set; }

        /// <summary>
        /// 金库任务数量
        /// </summary>
        public int? VtAffairsCount { get; set; }

        /// <summary>
        /// 线路数量
        /// </summary>
        public int? RoutesCount { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }


        public string Message { get; set; }
   }
}