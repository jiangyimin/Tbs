using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Tbs.Authorization;
using Tbs.Authorization.Users;
using Tbs.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Tbs.Authorization.Roles;
using Tbs.Roles.Dto;
using Abp.IdentityFramework;

namespace Tbs.Users
{
    /* THIS IS JUST A SAMPLE. */
    //[AbpAuthorize(PermissionNames.Admin_Manage)]
    public class UserAppService : TbsAppServiceBase, IUserAppService
    {
        private readonly string _defaultPassword = "123456";
        private readonly string _defaultEmailServer = "@126.com";

        private readonly RoleManager _roleManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserAppService(
            RoleManager roleManager, 
            IRepository<User, long> userRepository, 
            IPermissionManager permissionManager,
            IPasswordHasher<User> passwordHasher)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _passwordHasher = passwordHasher;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            await UserManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<ListResultDto<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
                ObjectMapper.Map<List<UserListDto>>(users.OrderBy(u => u.Name))
                );
        }

        public List<RoleListDto> GetRoles()
        {
            return ObjectMapper.Map<List<RoleListDto>>(
                _roleManager.Roles.OrderBy(t => t.Name).ToList()
            );
        }

        public async Task CreateUser(CreateUserInput input)
        {
            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Surname = user.UserName;
            user.Password = _passwordHasher.HashPassword(user, _defaultPassword);
            user.EmailAddress = input.UserName + _defaultEmailServer;
            user.IsEmailConfirmed = true;
            user.IsActive = true;

            await UserManager.CreateAsync(user);

            if (!string.IsNullOrEmpty(input.RoleName))
                await UserManager.AddToRoleAsync(user, input.RoleName);
        }

        public async Task UpdateUser(long userId, CreateUserInput input)
        {
            var user = await UserManager.GetUserByIdAsync(userId);

            user.Name = input.Name;
            string oldRole = user.RoleName;
            user.RoleName = input.RoleName;
            user.WhName = input.WhName;
            user.OperatePassword = input.OperatePassword;
            user.DepotSide = input.DepotSide == "on" ? true : false;

            await UserManager.UpdateAsync(user);
            if (oldRole != user.RoleName)
            {
                if (!string.IsNullOrEmpty(oldRole)) await UserManager.RemoveFromRoleAsync(user, oldRole);
                if (!string.IsNullOrEmpty(user.RoleName)) await UserManager.AddToRoleAsync(user, user.RoleName);         
            }
        }

        public async Task DeleteUser(long userId)
        {
            var user = await UserManager.GetUserByIdAsync(userId);
            await UserManager.DeleteAsync(user);       
        }

        public async Task ChangePassword(string password)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var identifyResult = await UserManager.ChangePasswordAsync(user, password);
            identifyResult.CheckErrors(LocalizationManager);

            return;
        }
        
        private async Task AddToRole(User user, string roleName)
        {
            var roles = GetRoles().Select(s => s.Name);
            await UserManager.RemoveFromRolesAsync(user, roles);
        }
    }
}