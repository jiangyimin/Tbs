using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// Customer Entity
    /// </summary>
    [Description("客户")]
    public class Customer : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 20;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// outlet definitions for this customer.
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual ICollection<Outlet> Outlets { get; set; }
    }     
}

