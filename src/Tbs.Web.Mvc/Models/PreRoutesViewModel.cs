using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Tbs.DomainModels.Dto;

namespace Tbs.Web.Models
{
    public class PreRoutesViewModel
    {
        public int DepotId { get; set; }
        public List<ComboboxItemDto> RouteTypes { get; set; }
        public string Finger { get; set; }

        public string OperatePassword {get; set; }
        public string PwdDeadline { get; set; }
    }
}