using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Validation;
using System.Collections.Generic;
using System;

namespace Tbs.DomainModels.Dto
{
    public class WeixinTaskBoxDto 
    {
        public string BoxCn { get; set; }

        public string Remark { get; set; }

        public WeixinTaskBoxDto(string boxCn, string remark)
        {
            BoxCn = boxCn;
            Remark = remark;
        }
    }

    public class WxOutletDto
    {
        public int TenantId { get; set; }

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

        public List<WeixinTaskBoxDto> TaskBoxs { get; set; }
        
        public WxOutletDto()
        {
            TenantId = 0;
            TaskBoxs = new List<WeixinTaskBoxDto>();
        }

        public void SetWorker(Worker worker)
        {
            if (worker == null) return;

            WorkerCn = worker.Cn;
            WorkerName = worker.Name;
            IDNumber = worker.IDNumber;
            Photo = worker.Photo != null ? Convert.ToBase64String(worker.Photo) : null;
        }

        public void SetWorker2(Worker worker)
        {
            if (worker == null) return;

            WorkerCn2 = worker.Cn;
            WorkerName2 = worker.Name;
            IDNumber2 = worker.IDNumber;
            Photo2 = worker.Photo != null ? Convert.ToBase64String(worker.Photo) : null;
        }

        public void SetVehice(Vehicle vehicle)
        {
            if (vehicle == null) return;

            VehicleCn = vehicle.Cn;
            License = vehicle.License;
            VehiclePhoto = vehicle.Photo != null ? Convert.ToBase64String(vehicle.Photo) : null;
        }

   }
}