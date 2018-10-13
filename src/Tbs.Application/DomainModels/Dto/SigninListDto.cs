using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMapFrom(typeof(Signin))]
    public class SigninListDto : EntityDto
    {
        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// 签到时段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        public string Worker { get; set; }
        
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SigninTime { get; set; }

        /// <summary>
        /// 迟到偏离量（- 为提前，按分钟）
        /// </summary>
        public int LateDistance { get; set; }
     }
}