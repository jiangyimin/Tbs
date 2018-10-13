using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Vehicle))]
    public class VehicleDto : InfoEntityDto, ICustomValidate, IShouldNormalize
    {
        [Required]
        public int DepotId { get; set; }

       /// <summary>
        /// 车牌编号
        /// </summary>
        [Required]
        public string Cn { get; set; }

        /// <summary>
        /// 车牌
        /// </summary>
        [Required]
        public string License { get; set; }

        public byte[] Photo { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public IFormFile PhotoFile { get; set; }

        public string Status { get; set; } = "正常";

        #region Interface implenments
        public override string Info { get { return $"车牌为{License}"; } }

        public void Normalize()
        {
            if (PhotoFile != null) 
            {
                Photo = new byte[PhotoFile.Length];
                PhotoFile.OpenReadStream().Read(Photo, 0, (int)PhotoFile.Length);
            }
        }
        public void AddValidationErrors(CustomValidationContext context)
        {
            const int V = 10240;
            if (PhotoFile != null && PhotoFile.Length > V)
                context.Results.Add(new ValidationResult("照片文件不能大于10K!"));
        }

        #endregion
    }
}