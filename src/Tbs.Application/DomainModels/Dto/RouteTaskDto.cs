using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(RouteTask))]
    public class RouteTaskDto : EntityDto
    {
        [Required]
        public int RouteId { get; set; }

        [Required]
        public string ArriveTime { get; set; }

        [Required]
        public int OutletId { get; set; }

        [Required]
        public int TaskTypeId { get; set; }

        public DateTime? IdentifyTime { get; set; }

        public int? Qtum { get; set; }

        public string Remark { get; set; }
    }
}