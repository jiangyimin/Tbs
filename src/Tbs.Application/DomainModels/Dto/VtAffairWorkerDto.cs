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
    [AutoMap(typeof(VtAffairWorker))]
    public class VtAffairWorkerDto : EntityDto, IShouldNormalize
    {
        [Required]
        public int VtAffairId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public int VaultRoleId { get; set; }
        public string VaultRoleName { get; set; }
        
        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public void Normalize()
        {
            DomainManager domainManager = IocManager.Instance.Resolve<DomainManager>();
            foreach (Worker worker in domainManager.GetWorkers(domainManager.GetDepotId()))
            {
                if (worker.Id == WorkerId)
                {
                    WorkerCn = worker.Cn;
                    WorkerName = worker.Name;
                    break;
                }
            }

            VaultRoleName = domainManager.GetVaultRole(VaultRoleId).Name;
        }
    }
}