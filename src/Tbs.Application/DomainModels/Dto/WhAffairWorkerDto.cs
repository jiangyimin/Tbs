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
    [AutoMap(typeof(WhAffairWorker))]
    public class WhAffairWorkerDto : EntityDto, IShouldNormalize
    {
        [Required]
        public int WhAffairId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }

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
                }
            }
        }
    }
}