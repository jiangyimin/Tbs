using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Runtime.Validation;
using Tbs.DomainModels;
using Tbs.DomainServices;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(WhAffair))]
    public class WhAffairDto : EntityDto, IShouldNormalize, ICustomValidate
    {
        [Required]
        public int DepotId { get; set; }
        
        [Required]
        public DateTime CarryoutDate { get; set; }

        [Required]
        public string WhName { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        public string Status { get; set; }

        public DateTime LastActiveTime { get; set; }

        public string Remark { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Status)) 
                Status = "安排";
        } 

        public void AddValidationErrors(CustomValidationContext context)
        {
            DomainManager domainManager = IocManager.Instance.Resolve<DomainManager>();

            if (string.Compare(StartTime, EndTime) >= 0)
                context.Results.Add(new ValidationResult("结束时间不能小于开始时间!"));
        }
      
    }
}