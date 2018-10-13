using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Tbs.Roles.Dto;

namespace Tbs.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        ListResultDto<RoleListDto> GetRoles(string tenancyName);

        Task UpdateRolePermissions(string tenancyName, UpdateRolePermissionsInput input);

        Task <List<string>> GetRolePermissionNames(string tenancyName, int roleId);

        Task CreateRole(string tenancyName, CreateRoleInput input);

        Task RemoveRole(string tenancyName, string roleName);
    }
}
