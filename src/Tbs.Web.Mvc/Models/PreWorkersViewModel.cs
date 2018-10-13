using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Tbs.DomainModels.Dto;

namespace Tbs.Web.Models
{
    public class PreWorkersViewModel
    {
        public int DepotId { get; set; }
        public List<RouteRoleDto> RouteRoles { get; set; }
    }
}