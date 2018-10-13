using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Validation;

namespace Tbs.DomainModels.Dto
{
    [AutoMap(typeof(Worker))]
    public class WorkerDto : InfoEntityDto, ICustomValidate, IShouldNormalize
    {
        /// <summary>
        /// 中心
        /// </summary>
        [Required]
        public int DepotId { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        [Required]
        public string Cn { get; set; }

       /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// IDCardNo 
        /// </summary>
        public string IDCardNo { get; set; }

        public byte[] Photo { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string Finger { get; set; }
        public string IDNumber { get; set; }
        public string Mobile { get; set; }
        public string DeviceId { get; set; }
        public string Status { get; set; } = "正常";


        #region
        public override string Info { get { return $"姓名为{Name}"; } }
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