using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Configuration;
using Tbs.Dto;

namespace Tbs.Settings
{
    public interface ISettingAppService : IApplicationService
    {
        List<PropertyDto> GetSettingsForApplication();
        List<PropertyDto> GetSettingsForTenant();

        Task ChangeSettingsForApplication(List<PropertyDto> settings);
        Task ChangeSettingsForTenant(List<PropertyDto> settings);
    }
}
