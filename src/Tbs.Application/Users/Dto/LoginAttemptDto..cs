using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;

namespace Tbs.Users.Dto
{
    [AutoMap(typeof(UserLoginAttempt))]
    public class LoginAttemptDto : InfoEntityDto<long>
    {
        public virtual string UserNameOrEmailAddress { get; set; }
        //
        // Summary:
        //     /// IP address of the client. ///
        public virtual string ClientIpAddress { get; set; }
        //
        // Summary:
        //     /// Name (generally computer name) of the client. ///
        public virtual string ClientName { get; set; }
        //
        // Summary:
        //     /// Browser information if this method is called in a web request. ///
        [MaxLength(256)]
        public virtual string BrowserInfo { get; set; }
        //
        // Summary:
        //     /// Login attempt result. ///
        public virtual AbpLoginResultType Result { get; set; }
        public virtual DateTime CreationTime { get; set; }
    }
}