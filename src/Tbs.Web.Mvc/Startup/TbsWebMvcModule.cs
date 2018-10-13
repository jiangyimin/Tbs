using System.Reflection;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Tbs.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Tbs.MultiTenancy;
using Abp.Configuration;
using System.Linq;

namespace Tbs.Web.Startup
{
    [DependsOn(typeof(TbsWebCoreModule))]
    public class TbsWebMvcModule : AbpModule
    {
        public TenantManager TenantManager { protected get; set; }
        public SettingManager SettingManager { protected get; set; }
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TbsWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<TbsNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TbsWebMvcModule).GetAssembly());
        }
    }
}