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
    [AutoMap(typeof(VtAffair))]
    public class VtAffairDto : EntityDto, IShouldNormalize, ICustomValidate
    {
        [Required]
        public int DepotId { get; set; }
        
        [Required]
        public DateTime CarryoutDate { get; set; }

        [Required]
        public int VaultTypeId { get; set; }
        
        [Required]
        public string VtName { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        public string Status { get; set; }
        public string ActivateInfo { get; set; }

        public DateTime LastActiveTime { get; set; }

        public string Remark { get; set; }

        #region
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Status)) 
                Status = "安排";
        }       

        public void AddValidationErrors(CustomValidationContext context)
        {
            DomainManager domainManager = IocManager.Instance.Resolve<DomainManager>();

            VaultType vaultType = domainManager.GetVaultType(VaultTypeId);
            
            if (string.Compare(vaultType.EarliestTime, StartTime) > 0)
                context.Results.Add(new ValidationResult("开始时间不能早于金库操作类型的最早时间!"));

            if (string.Compare(vaultType.LatestTime, EndTime) < 0)
                context.Results.Add(new ValidationResult("结束时间不能晚于金库操作类型的最晚时间!"));

            if (string.Compare(StartTime, EndTime) >= 0)
                context.Results.Add(new ValidationResult("结束时间不能小于开始时间!"));
        }

        #endregion      
    }
}