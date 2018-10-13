using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;

namespace Tbs.Web.Models
{
    public class RoutesViewModel
    {
        public string Today { get; set; }
        public int DepotId { get; set; }

        public string Finger { get; set; }

        public string OperatePassword {get; set; }
        public string PwdDeadline { get; set; }
        public List<RouteRole> RouteRoles { get; set; }

        public List<Depot> UserDepots { get; set; }
    }
}