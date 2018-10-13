using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.Roles.Dto;
using Tbs.Users.Dto;

namespace Tbs.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        List<RoleListDto> GetRoles();

        Task<ListResultDto<UserListDto>> GetUsers();

        Task CreateUser(CreateUserInput input);
        Task UpdateUser(long userId, CreateUserInput input);
        Task DeleteUser(long userId);

        Task ChangePassword(string password);
    }
}