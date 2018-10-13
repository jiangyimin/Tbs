using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Http;
using Tbs.Authorization;
using Tbs.DomainModels;
using Tbs.DomainModels.Dto;

namespace Tbs
{
    [DependsOn(
        typeof(TbsCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class TbsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<TbsAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                // common
                mapper.CreateMap<string, bool>().ConvertUsing(s => s == "on" ? true : false);
                mapper.CreateMap<bool, string>().ConvertUsing(s => s ? "on" : "");

                mapper.CreateMap<VehicleDto, Vehicle>().ForMember(d => d.Photo, opt => opt.Condition(s => s.Photo != null));
                mapper.CreateMap<WorkerDto, Worker>().ForMember(d => d.Photo, opt => opt.Condition(s => s.Photo != null));
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TbsApplicationModule).GetAssembly());
        }
    }
}