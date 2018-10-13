using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(PreRouteTask))]
    public class PreRouteTaskDto : EntityDto
    {
        [Required]
        public int PreRouteId { get; set; }

        [Required]
        public string ArriveTime { get; set; }

        [Required]
        public int OutletId { get; set; }

        [Required]
        public int TaskTypeId { get; set; }
    }
}