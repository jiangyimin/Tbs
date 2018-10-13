using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Tbs.Authorization.Roles;
using Tbs.Authorization.Users;
using Tbs.Configuration;
using Tbs.Identity;
using Tbs.MultiTenancy;
using Tbs.Web.Resources;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#if FEATURE_SIGNALR
using Owin;
using Abp.Owin;
using Tbs.Owin;
#endif

namespace Tbs.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAbpIdentity<Tenant, User, Role, SecurityStampValidator>(options =>
                {
                    options.Cookies.ApplicationCookie.AuthenticationScheme = "TbsAuthSchema";
                    options.Cookies.ApplicationCookie.CookieName = "TbsAuth";
                })
                .AddUserManager<UserManager>()
                .AddRoleManager<RoleManager>()
                .AddSignInManager<SignInManager>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();


            services.AddScoped<IWebResourceManager, WebResourceManager>();

            services.AddSession(o => o.IdleTimeout = TimeSpan.FromSeconds(100));

            //Configure Abp and Dependency Injection
            return services.AddAbp<TbsWebMvcModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            AuthConfigurer.Configure(app, _appConfiguration);

            app.UseStaticFiles();
            app.UseSession();       // must befor UseMvc.

#if FEATURE_SIGNALR
            //Integrate to OWIN
            app.UseAppBuilder(ConfigureOwinServices);
#endif
            app.UseMvc(routes =>
            {
                // routes.MapRoute(
                //     name: "defaultWithArea",
                //     template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            WeixinRegistar.Register();
        }

#if FEATURE_SIGNALR
        private static void ConfigureOwinServices(IApplicationBuilder app)
        {
            app.Properties["host.AppName"] = "Tbs";

            app.UseAbp();

            app.MapSignalR();
        }
#endif

    }
}
