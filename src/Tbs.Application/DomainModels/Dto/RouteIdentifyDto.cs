using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(RouteIdentify))]
    public class RouteIdentifyDto : EntityDto
    {
        [Required]
        public int RouteId { get; set; }

        [Required]
        public int OutletId { get; set; }

        public DateTime? IdentifyTime { get; set; }

    }
}