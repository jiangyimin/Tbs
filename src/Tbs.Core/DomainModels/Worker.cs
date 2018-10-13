using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Worker Entity
    /// </summary>
    [Description("工作人员")]
    public class Worker : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        public const int MaxCnLength = 8;
        public const int MaxNameLength = 8;
        public const int MaxPasswordLength = 10;
        public const string PasswordRegex = "^[0-9]{6}$";
        public const int IDCardNoMaxLength = 18;
        public const int IDNumberLength = 18;
        public const int MobileLength = 11;
        public const int FingerLength = 1024;
        public const int MaxDeviceId = 50;
 
        /// <summary>
        /// 所属中心
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        
        [ForeignKey("DepotId")]
        public Depot Depot { get; set; }
        
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 指纹
        /// </summary>
        [StringLength(FingerLength)]
        public string Finger { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [StringLength(IDNumberLength)]
        public string IDNumber { get; set; }

        /// <summary>
        /// IDCardNo 
        /// </summary>
        [StringLength(IDCardNoMaxLength)]
        public string IDCardNo { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(MobileLength)]
        public string Mobile { get; set; }

        /// <summary>
        /// 微信设备Id
        /// </summary>
        [StringLength(MaxDeviceId)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(TbsConsts.StatusLength)]        
        public string Status;
    }
}

