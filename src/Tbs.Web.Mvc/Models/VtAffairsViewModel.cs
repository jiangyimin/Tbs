using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;

namespace Tbs.Web.Models
{
    public class VtAffairsViewModel
    {
        public string Today { get; set; }
        public int DepotId { get; set; }

        public List<string> VtNames { get; set; }

        public string Finger { get; set; }

        public string OperatePassword {get; set; }
        public string PwdDeadline { get; set; }
        public List<VaultRole> VaultRoles { get; set; }

        public List<Depot> UserDepots { get; set; }
    }
}