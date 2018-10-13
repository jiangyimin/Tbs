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
    [AutoMap(typeof(Vehicle))]
    public class PreWorkerDto : InfoEntityDto
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
        public int? Worker1Id { get; set; }

        public int? Worker2Id { get; set; }

         public int? Worker3Id { get; set; }

        public int? Worker4Id { get; set; }

        public int? Worker5Id { get; set; }

        public int? Worker6Id { get; set; }    

    }
}