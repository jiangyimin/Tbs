using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Article))]
    public class ArticleDto : InfoEntityDto
    {
        /// <summary>
        /// 物流中心
        /// </summary>
        public int DepotId { get; set; }
         /// <summary>
        /// 编号
        /// </summary>
        [Required]
        public string Cn { get; set; }

        /// <summary>
        /// 物品名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [Required]
        public int ArticleTypeId { get; set; }

        /// <summary>
        /// Rfid 
        /// </summary>
        public string Rfid { get; set; }

        /// <summary>
        /// 绑定信息 
        /// </summary>
        public string BindInfo { get; set; }
        
        #region Interface implenments
        public override string Info { get { return $"名称为{Name}"; } }

        #endregion
    }
}