using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(RouteRole))]
    public class RouteRoleDto : InfoEntityDto
    {
        /// <summary>
        /// Foreign Id of the RouteType.
        /// </summary>
        public int RouteTypeId { get; set; }
       
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 是否必须
        /// </summary>
        public string Required { get; set; }

        /// <summary>
        /// 可领用物品列表(类型 MAX)，比如(枪 1)(弹 2)
        /// </summary>
        public string ArticleTypeList { get; set; }
          
        /// <summary>
        /// 同组号
        /// </summary>
        public string PeerGroupNo { get; set; }

        #region Interface implenments
        public override string Info { get { return $"角色为{Name}"; } }

        #endregion
    }
}