using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.Web.Models
{
    public class KeeperViewModel
    {
        public string Today { get; set; }
        public int DepotId { get; set; }

        public Warehouse Warehouse { get; set; }   // for vtAffairCheck

        public string Keepers { get; set; }
        public int WhAffairId { get; set; }

        public List<KeyValuePair<int, string>> Depots { get; set; }   // for routesCheck
    }
}