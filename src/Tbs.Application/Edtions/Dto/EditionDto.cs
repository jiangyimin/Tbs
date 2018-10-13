using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Editions;
using Abp.Application.Services.Dto;

namespace Tbs.Editions.Dto
{
    [AutoMap(typeof(Edition))]
    public class EditionDto : EntityDto
    {
        [Required]
        [StringLength(Edition.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(Edition.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
     }
}