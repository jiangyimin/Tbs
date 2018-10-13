using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Vehicle))]
    public class VehicleWorkerDto : InfoEntityDto
    {
        [Required]
        public int DepotId { get; set; }

       /// <summary>
        /// 车牌编号
        /// </summary>
        [Required]
        public string Cn { get; set; }

        /// <summary>
        /// 车牌
        /// </summary>
        [Required]
        public string License { get; set; }

        /// <summary>
        ///  身份确认人员
        /// </summary>
        public int? MainWorkerId { get; set; }

        public int? SubWorkerId { get; set; }
    }
}