using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(TaskType))]
    public class TaskTypeDto : InfoEntityDto
    {
        [Required]
        public string Cn { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string GroupName { get; set; }

        /// <summary>
        /// 基本价格
        /// </summary>
        public int BasicPrice { get; set; }


        public override string Info { get { return $"名称为{Name}"; } }
     }
}