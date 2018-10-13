using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// PreWorkers Entity
    /// </summary>
    public class PreWorker : Entity, IMustHaveTenant
    {
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
        [StringLength(PreRoute.MaxCnLength)]
        public string RouteCn { get; set; }

        /// <summary>
        ///  身份确认人员
        /// </summary>
        public int? Worker1Id { get; set; }
        public int? Worker2Id { get; set; }
        public int? Worker3Id { get; set; }

        public int? Worker4Id { get; set; }
        public int? Worker5Id { get; set; }
        public int? Worker6Id { get; set; }

    }
}

