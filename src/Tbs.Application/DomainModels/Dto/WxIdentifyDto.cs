using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Validation;
using System.Collections.Generic;

namespace Tbs.DomainModels.Dto
{
    public class WeixinTaskDto 
    {
        public int TaskId { get; set; }
        public string ArriveTime { get; set; }
        public string TaskType { get; set; }

        public string OutletCn { get; set; }
        public string OutletName { get; set; }

        public string IdentifyTime { get; set; }

        public string Remark { get; set; }

        public WeixinTaskDto(int taskId, string arriveTime, string taskType, string outletCn, string outletName, string identifyTime, string remark)
        {
            TaskId = taskId;
            ArriveTime = arriveTime;
            TaskType = taskType;
            OutletCn = outletCn;
            OutletName = outletName;
            IdentifyTime = identifyTime;
            Remark = remark;
        }
    }

    public class WxIdentifyDto
    {
        public int TenantId { get; set; }
        public int DepotId { get; set; }

        public string WorkerCn { get; set; }

        public string WorkerName { get; set; }
        public string IDNumber { get; set; }
        public string Photo { get; set; }

        public string WorkerCn2 { get; set; }

        public string WorkerName2 { get; set; }
        public string IDNumber2 { get; set; }
        public string Photo2 { get; set; }


        /// <summary>
        /// 车辆
        /// </summary>
        public string VehicleCn { get; set; }
        public string License { get; set; }
        public string VehiclePhoto { get; set; }

        // Route and Tasks
        public bool UseRoute { get; set; }

        public int RouteId { get; set; }
        public List<WeixinTaskDto> Tasks { get; set; }
        public int TaskId { get; set; }
        public string OutletCn { get; set; }
        public string OutletName { get; set; }
        public string OutletPassword { get; set; }
        public string OutletCipertext { get; set; }

        // Options
        public string ErrorMessage { get; set; }

        public WxIdentifyDto(int tenantId, string workerCn)
        {
            TenantId = tenantId;
            WorkerCn = workerCn;
            Tasks = new List<WeixinTaskDto>();
       }

   }
}