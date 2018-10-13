using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(DepotSignin))]
    public class DepotSigninDto : InfoEntityDto
    {
        /// <summary>
        /// 物流中心
        /// </summary>
        public int DepotId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string  EndTime { get; set; }

        [Required]
        public string  LateTime { get; set; }

        public override string Info { get { return $"名称为{Name}"; } }
     }
}