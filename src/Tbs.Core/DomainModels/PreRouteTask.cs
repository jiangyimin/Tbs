using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Tbs.DomainModels
{
    /// <summary>
    /// PreRouteTask Entity
    /// </summary>
    [Description("预排线路任务")]
    public class PreRouteTask : Entity
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int PreRouteId { get; set; }

        /// <summary>
        /// 预计达到时间
        /// </summary>
        [Required]
        [StringLength(TbsConsts.MSTimeLength)]
        public string ArriveTime { get; set; }

        /// <summary>
        /// 外键：银行网点
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }

        /// <summary>
        /// 外键：任务类型
        /// </summary>
        [Required]
        public int TaskTypeId { get; set; }
        public virtual TaskType TaskType { get; set; }

    }
}

