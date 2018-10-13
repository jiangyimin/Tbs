using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Tbs.Authorization.Users;

namespace Tbs.Users.Dto
{
    [AutoMap(typeof(User))]
    public class CreateUserInput
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        public string DepotSide { get; set; }

        public string RoleName { get; set; }

        public string WhName { get; set; }

        public string OperatePassword { get; set; }

        // other
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; } 
    }
}