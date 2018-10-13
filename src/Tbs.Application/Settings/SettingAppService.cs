using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.MultiTenancy;
using Abp.UI;
using Abp.Web.Models;
using Abp.Zero.Configuration;
using Tbs.Configuration;
using Tbs.Dto;

namespace Tbs.Settings
{
    /* THIS IS JUST A SAMPLE. */
    public class SettingAppService : TbsAppServiceBase, ISettingAppService
    {
        private readonly TbsCoreSettingProvider _provider;
        public SettingAppService(TbsCoreSettingProvider provider)
        {
            _provider = provider;
        }
        public List<PropertyDto> GetSettingsForApplication()
        {
            List<PropertyDto> lst = new List<PropertyDto>();
            foreach (SettingDefinition sd in _provider.GetSettingDefinitions(null).Where(sd => sd.Scopes == SettingScopes.Application))
            {
                string v = SettingManager.GetSettingValueForApplication(sd.Name);
                lst.Add(new PropertyDto(sd, v));              
            }
            return lst;
        }
        public List<PropertyDto> GetSettingsForTenant()
        {
            int tenantId = AbpSession.TenantId.Value;
            List<PropertyDto> lst = new List<PropertyDto>();
            foreach (SettingDefinition sd in _provider.GetSettingDefinitions(null).Where(sd => sd.Scopes.HasFlag(SettingScopes.Tenant)))
            {
                string v = SettingManager.GetSettingValueForTenant(sd.Name, tenantId);
                lst.Add(new PropertyDto(sd, v));              
            }
            return lst;
        }

        public async Task ChangeSettingsForApplication(List<PropertyDto> settings)
        {
            foreach(PropertyDto p in settings)
            {
                string name = p.Name.Split(' ')[0];
                await SettingManager.ChangeSettingForApplicationAsync(name, p.Value);
            }
            
        }

        public async Task ChangeSettingsForTenant(List<PropertyDto> settings)
        {
            int tenantId = AbpSession.TenantId.Value;
            foreach(PropertyDto p in settings)
            {
                string name = p.Name.Split(' ')[0];
                await SettingManager.ChangeSettingForTenantAsync(tenantId, name, p.Value);
            }
            
        }
    }
}