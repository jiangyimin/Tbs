using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Outlet))]
    public class OutletDto : InfoEntityDto
    {
        /// <summary>
        /// 物流中心
        /// </summary>
        public int? DepotId { get; set; }
        public Depot Depot { get; set; }

        public int? CustomerId { get; set; }

        [Required]
        public string Cn { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [RegularExpression(Outlet.PasswordRegex)]
        public string Password { get; set; }

        /// <summary>
        /// 密文
        /// </summary>
        [RegularExpression(Outlet.PasswordRegex)]
        public string Ciphertext { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }

        #region Interface implenments
        public override string Info { get { return $"名称为{Name}"; } }

        #endregion
    }
}