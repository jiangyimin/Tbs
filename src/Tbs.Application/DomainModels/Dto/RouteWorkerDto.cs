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
    [AutoMap(typeof(RouteWorker))]
    public class RouteWorkerDto : EntityDto, IShouldNormalize
    {
        [Required]
        public int RouteId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public int RouteRoleId {get; set; }
        public string RouteRoleName { get; set; }

        public string RecordList { get; set; }
        
        public DateTime? LendTime { get; set; }

        public DateTime? ReturnTime { get; set; }

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

            RouteRoleName = domainManager.GetRouteRole(RouteRoleId).Name;
        }
    }
}