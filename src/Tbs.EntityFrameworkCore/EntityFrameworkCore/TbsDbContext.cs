using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Tbs.Authorization.Roles;
using Tbs.Authorization.Users;
using Tbs.MultiTenancy;
using Tbs.DomainModels;

namespace Tbs.EntityFrameworkCore
{
    public class TbsDbContext : AbpZeroDbContext<Tenant, Role, User, TbsDbContext>
    {
        // Company
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<RouteType> RouteTypes { get; set; }
        public DbSet<RouteRole> RouteRoles { get; set; }
        public DbSet<VaultType> VaultTypes { get; set; }
        public DbSet<VaultRole> VaultRoles { get; set; }
        // public DbSet<Customer> Customers { get; set; }
        public DbSet<Outlet> Outlets { get; set; }

        // Depot and Depot Resources
        public DbSet<Depot> Depots { get; set; }
        public DbSet<DepotSignin> DepotSignins { get; set; }
        public DbSet<DepotFeature> DepotFeatures { get; set; }
        public DbSet<DepotSetting> DepotSettings { get; set; }

        public DbSet<Vault> Vaults { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Manager> Managers { get; set; }

        // Routine
        public DbSet<Signin> Signins { get; set; }
        public DbSet<WhAffair> WhAffairs { get; set; }
        public DbSet<WhAffairWorker> WhAffairWorkers { get; set; }
        public DbSet<VtAffair> VtAffairs { get; set; }
        public DbSet<VtAffairWorker> VtAffairWorkers { get; set; }
        public DbSet<PreRoute> PreRoutes { get; set; }
        public DbSet<PreRouteTask> PreRouteTasks { get; set; }
        
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteLog> RouteLogs { get; set; }
        public DbSet<RouteWorker> RouteWorkers { get; set; }
        public DbSet<RouteTask> RouteTasks { get; set; }
        public DbSet<RouteIdentify> RouteIdentifies { get; set; }
        public DbSet<ArticleRecord> ArticleRecords { get; set; }
        
        public DbSet<DaySettle> DaySettles { get; set; }
        public TbsDbContext(DbContextOptions<TbsDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Outlet>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });
            modelBuilder.Entity<Worker>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });
            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });
            //...

            modelBuilder.Entity<Signin>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.CarryoutDate, e.Name});
            });

            modelBuilder.Entity<PreRoute>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.RouteTypeId});
            });

            modelBuilder.Entity<WhAffair>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.CarryoutDate});
            });

            modelBuilder.Entity<VtAffair>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.CarryoutDate});
            });

            modelBuilder.Entity<Route>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.CarryoutDate});
            });

            modelBuilder.Entity<DaySettle>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.CarryoutDate});
            });

            modelBuilder.Entity<ArticleRecord>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.ArticleId});
            });
        }
    }
}
