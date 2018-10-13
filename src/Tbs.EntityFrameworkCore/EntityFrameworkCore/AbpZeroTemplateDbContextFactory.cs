using Tbs.Configuration;
using Tbs.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Tbs.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class TbsDbContextFactory : IDbContextFactory<TbsDbContext>
    {
        public TbsDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<TbsDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            TbsDbContextConfigurer.Configure(builder, configuration.GetConnectionString(TbsConsts.ConnectionStringName));
            
            return new TbsDbContext(builder.Options);
        }
    }
}