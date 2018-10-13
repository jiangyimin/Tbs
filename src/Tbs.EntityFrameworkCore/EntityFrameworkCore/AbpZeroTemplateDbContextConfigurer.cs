using Microsoft.EntityFrameworkCore;

namespace Tbs.EntityFrameworkCore
{
    public static class TbsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TbsDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, b => b.UseRowNumberForPaging());
        }
    }
}