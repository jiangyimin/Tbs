using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Tbs.Authorization.Roles;

namespace Tbs.Roles.Dto
{
    [AutoMapFrom(typeof(Role))]
    public class RoleListDto : EntityDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}