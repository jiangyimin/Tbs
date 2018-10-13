using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Runtime.Session;
using Abp.Configuration;
using Tbs.DomainServices;
using Tbs.Configuration;
using Tbs.Sessions;

namespace Tbs.Web.Views.Shared.Components.LoginUserInfo
{
    public class LoginUserInfoViewComponent : ViewComponent
    {
        private readonly IAbpSession _abpSession;
        private readonly ISettingManager _settingManager;
        private readonly DomainManager _domainManager;

        public LoginUserInfoViewComponent(
            IAbpSession abpSession, 
            ISettingManager settingManager, 
            DomainManager domainManager)
        {
            _abpSession = abpSession;
            _settingManager = settingManager;
            _domainManager = domainManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            LoginUserInfoViewModel model = new LoginUserInfoViewModel();

            if (_abpSession.TenantId.HasValue)
            {
                model.CompanyImageName = await _settingManager.GetSettingValueForTenantAsync(SettingNames.UI.CompanyImageName, _abpSession.TenantId.Value);
                model.CompanyName = await _settingManager.GetSettingValueForTenantAsync(SettingNames.UI.CompanyName, _abpSession.TenantId.Value);
            }
            model.UserDisplayInfo = _domainManager.GetUserDisplayInfo();

            return View(model);
        }
    }
}
