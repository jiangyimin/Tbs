using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Outlet Entity
    /// </summary>
    [Description("网点")]
    public class Outlet : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 8;
        public const int MaxNameLength = 20;
        public const int MaxPasswordLength = 10;
        public const string PasswordRegex = "^[0-9]{6}$";

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 专属中心
        /// </summary>
        public int? DepotId { get; set; }
        public Depot Depot { get; set; }


        /// <summary>
        /// 所属客户Id
        /// </summary>
        public int? CustomerId { get; set; }

        
        /// <summary>
        /// 网点编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }
        
        /// <summary>
        /// 网点名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 交接密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 交接密文
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Ciphertext { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Longitude { get; set; }

    }
}

