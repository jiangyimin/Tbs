using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Validation;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Manager))]
    public class ManagerDto : InfoEntityDto
    {
        /// <summary>
        /// 人员编号
        /// </summary>
        [Required]
        public string Cn { get; set; }

       /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public override string Info { get { return $"姓名为{Name}"; } }
   }
}