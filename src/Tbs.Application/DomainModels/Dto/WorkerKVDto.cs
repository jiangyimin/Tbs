using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Validation;

namespace Tbs.DomainModels.Dto
{
    [AutoMapFrom(typeof(Worker))]
    public class WorkerKVDto : EntityDto
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

        /// <summary>
        /// IDCardNo 
        /// </summary>
        public string IDCardNo { get; set; }

        public string DisplayText { get { return Cn + " " + Name; } }
   }
}