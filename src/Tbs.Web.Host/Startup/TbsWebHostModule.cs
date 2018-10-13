using System.Reflection;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Tbs.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tbs.Web.Host.Startup
{
    [DependsOn(
       typeof(TbsWebCoreModule))]
    public class TbsWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public TbsWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TbsWebHostModule).GetAssembly());
        }
    }
}
