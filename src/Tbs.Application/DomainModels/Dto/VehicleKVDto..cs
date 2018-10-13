using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;

namespace Tbs.DomainModels.Dto
{
    [AutoMapFrom(typeof(Vehicle))]
    public class VehicleKVDto : EntityDto
    {
        [Required]
        public string Cn { get; set; }

        [Required]
        public string License { get; set; }

        public string DisplayText { get { return Cn + " " + License; } }
    }
}