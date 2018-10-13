using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.MultiTenancy;
using Abp.UI;
using Abp.Web.Models;
using Abp.Zero.Configuration;
using Tbs.Authorization.Roles;
using Tbs.Roles.Dto;

namespace Tbs.Roles
{
    /* THIS IS JUST A SAMPLE. */
    public class RoleAppService : TbsAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        private readonly IRoleManagementConfig _roleManagementConfig;
        
        public RoleAppService(RoleManager roleManager, IPermissionManager permissionManager, IRoleManagementConfig roleManagementConfig)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _roleManagementConfig = roleManagementConfig;
        }

        public ListResultDto<RoleListDto> GetRoles(string tenancyName)
        {
            var tenant = TenantManager.FindByTenancyNameAsync(tenancyName).Result;
            if (tenant == null) 
                return null;

            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                return new ListResultDto<RoleListDto>(
                    ObjectMapper.Map<List<RoleListDto>>(
                        _roleManager.Roles.OrderBy(t => t.Name).ToList()
                    )
                );
            }
        }

        public async Task UpdateRolePermissions(string tenancyName, UpdateRolePermissionsInput input)
        {
            if (string.IsNullOrEmpty(tenancyName))
                return;
            
            var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
                await _roleManager.ResetAllPermissionsAsync(role);
                var grantedPermissions = _permissionManager
                    .GetAllPermissions(false)
                    .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                    .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            }
        }

        public async Task<List<string>> GetRolePermissionNames(string tenancyName, int roleId)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
            List<string> lst = new List<string>();
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                var permissions = await _roleManager.GetGrantedPermissionsAsync(roleId);
                foreach (Permission p in permissions)
                    lst.Add(p.Name);
            }
            return lst;
        }

        public async Task CreateRole(string tenancyName, CreateRoleInput input)
        {
            if (string.IsNullOrEmpty(tenancyName))
                return;
            
            var staticRoleDefinitions = _roleManagementConfig.StaticRoles.Where(sr => sr.Side == MultiTenancySides.Tenant  && sr.RoleName == input.Name);
            if (staticRoleDefinitions.Count() == 0)
                throw new UserFriendlyException("不能创建非预设角色");
                
            var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {

                //Create role
                var role = ObjectMapper.Map<Role>(input);
                role.IsStatic = true;
                await _roleManager.CreateAsync(role);
            }
        }

        public async Task RemoveRole(string tenancyName, string roleName)
        {
            if (string.IsNullOrEmpty(tenancyName))
                return;
            
            var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                await _roleManager.DeleteAsync(role);
            }
        }

    }
}