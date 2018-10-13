using System.Collections.Generic;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;

namespace Tbs.Web.Models
{
    public class SigninsViewModel
    {
        public int DepotId { get; set; }
        public string Today { get; set; }
        public List<DepotSigninDto> Names { get; set; }

        public List<Depot> UserDepots { get; set; }
    }
}