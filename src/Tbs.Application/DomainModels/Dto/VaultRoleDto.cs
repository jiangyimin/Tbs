using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(VaultRole))]
    public class VaultRoleDto : InfoEntityDto
    {
        /// <summary>
        /// Foreign Id of the VaultType.
        /// </summary>
        public int VaultTypeId { get; set; }
       
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
        /// 是否单个
        /// </summary>
        public string Single { get; set; }
          
        #region Interface implenments
        public override string Info { get { return $"角色为{Name}"; } }

        #endregion
    }
}