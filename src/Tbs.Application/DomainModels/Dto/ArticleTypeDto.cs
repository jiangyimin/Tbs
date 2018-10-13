using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(ArticleType))]
    public class ArticleTypeDto : InfoEntityDto
    {
        [Required]
        public string Cn { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string BindTo { get; set; }

        public override string Info { get { return $"名称为{Name}"; } }
     }
}