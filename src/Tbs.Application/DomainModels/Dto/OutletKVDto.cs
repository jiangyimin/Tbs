using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMapFrom(typeof(Outlet))]
    public class OutletKVDto : EntityDto
    {
        [Required]
        public string Cn { get; set; }

        [Required]
        public string Name { get; set; }

        public string DisplayText { get { return Cn + " " + Name; } }
    }
}