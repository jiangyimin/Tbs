using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Depot))]
    public class DepotDto : InfoEntityDto
    {
        [Required]
        public string Cn { get; set; }
        [Required]
        public string Name { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string UseRouteForIdentify { get; set; }

        #region Interface implenments
        public override string Info { get { return $"名称为{Name}"; } }

        #endregion
     }
}