using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Vehicle Entity
    /// </summary>
    [Description("押运车辆")]
    public class Vehicle : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 8;
        public const int LicenseLength = 7;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        [ForeignKey("DepotId")]
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [Required]
        [StringLength(LicenseLength)]
        public string License { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }

        [Required]
        [StringLength(TbsConsts.StatusLength)]        
        public string Status;

        /// <summary>
        ///  身份确认人员
        /// </summary>
        public int? MainWorkerId { get; set; }
        public int? SubWorkerId { get; set; }


        /// <summary>
        ///  车组人员
        /// </summary>
        public int? Worker1Id { get; set; }
        public int? Worker2Id { get; set; }
        public int? Worker3Id { get; set; }

        public int? Worker4Id { get; set; }
        public int? Worker5Id { get; set; }
        public int? Worker6Id { get; set; }

    }
}

