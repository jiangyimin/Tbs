using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Tbs.EntityFrameworkCore.Seed;

namespace Tbs.EntityFrameworkCore
{
    [DependsOn(
        typeof(TbsCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class TbsEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }


        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<TbsDbContext>(configuration =>
                {
                    TbsDbContextConfigurer.Configure(configuration.DbContextOptions, configuration.ConnectionString);
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TbsEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}