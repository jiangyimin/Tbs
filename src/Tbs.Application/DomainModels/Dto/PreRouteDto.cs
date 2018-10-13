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
    [AutoMap(typeof(PreRoute))]
    public class PreRouteDto : EntityDto, ICustomValidate
    {
        [Required]
        public int DepotId { get; set; }
        [Required]
        public int RouteTypeId { get; set; }
        [Required]
        public string RouteCn { get; set; }
        [Required]
        public string RouteName { get; set; }

        [Required]
        public string SetoutTime { get; set; }

        [Required]
        public string ReturnTime { get; set; }

        public int? VehicleId { get; set; }

        #region

        public void AddValidationErrors(CustomValidationContext context)
        {
            DomainManager domainManager = IocManager.Instance.Resolve<DomainManager>();

            RouteType routeType = domainManager.GetRouteType(RouteTypeId);
            
            if (string.Compare(routeType.EarliestTime, SetoutTime) > 0)
                context.Results.Add(new ValidationResult("出发时间不能早于线路类型的最早时间!"));

            if (string.Compare(routeType.LatestTime, ReturnTime) < 0)
                context.Results.Add(new ValidationResult("返回时间不能晚于线路类型的最晚时间!"));
        }

        #endregion
        
    }
}