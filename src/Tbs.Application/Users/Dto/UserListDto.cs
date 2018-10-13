using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.Authorization.Users;

namespace Tbs.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        // Tbs User Items
        public string DepotSide { get; set; }

        public string RoleName { get; set; }
        public string WhName { get; set; }
        public string OperatePassword { get; set; }

    }
}