using Abp.Application.Services.Dto;
using Tbs.Editions.Dto;
using System.Collections.Generic;
using Abp.Application.Features;

namespace Tbs.Web.Models
{
    public class EditionsViewModel
    {
        public ListResultDto<EditionDto> Editions { get; set; }
        public IReadOnlyList<Feature> Features { get; set; }
    }
}