using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tbs.EntityFrameworkCore;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Notifications;

namespace Tbs.Migrations
{
    [DbContext(typeof(TbsDbContext))]
    [Migration("20170709133845_m3")]
    partial class m3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Abp.Application.Editions.Edition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("AbpEditions");
                });

            modelBuilder.Entity("Abp.Application.Features.FeatureSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.ToTable("AbpFeatures");

                    b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
                });

            modelBuilder.Entity("Abp.Auditing.AuditLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrowserInfo")
                        .HasMaxLength(256);

                    b.Property<string>("ClientIpAddress")
                        .HasMaxLength(64);

                    b.Property<string>("ClientName")
                        .HasMaxLength(128);

                    b.Property<string>("CustomData")
                        .HasMaxLength(2000);

                    b.Property<string>("Exception")
                        .HasMaxLength(2000);

                    b.Property<int>("ExecutionDuration");

                    b.Property<DateTime>("ExecutionTime");

                    b.Property<int?>("ImpersonatorTenantId");

                    b.Property<long?>("ImpersonatorUserId");

                    b.Property<string>("MethodName")
                        .HasMaxLength(256);

                    b.Property<string>("Parameters")
                        .HasMaxLength(1024);

                    b.Property<string>("ServiceName")
                        .HasMaxLength(256);

                    b.Property<int?>("TenantId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId", "ExecutionDuration");

                    b.HasIndex("TenantId", "ExecutionTime");

                    b.HasIndex("TenantId", "UserId");

                    b.ToTable("AbpAuditLogs");
                });

            modelBuilder.Entity("Abp.Authorization.PermissionSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsGranted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId", "Name");

                    b.ToTable("AbpPermissions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
                });

            modelBuilder.Entity("Abp.Authorization.Roles.RoleClaim", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<int>("RoleId");

                    b.Property<int?>("TenantId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.HasIndex("TenantId", "ClaimType");

                    b.ToTable("AbpRoleClaims");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("EmailAddress");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.Property<long?>("UserLinkId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress");

                    b.HasIndex("UserName");

                    b.HasIndex("TenantId", "EmailAddress");

                    b.HasIndex("TenantId", "UserId");

                    b.HasIndex("TenantId", "UserName");

                    b.ToTable("AbpUserAccounts");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserClaim", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TenantId", "ClaimType");

                    b.ToTable("AbpUserClaims");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserLogin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TenantId", "UserId");

                    b.HasIndex("TenantId", "LoginProvider", "ProviderKey");

                    b.ToTable("AbpUserLogins");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserLoginAttempt", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrowserInfo")
                        .HasMaxLength(256);

                    b.Property<string>("ClientIpAddress")
                        .HasMaxLength(64);

                    b.Property<string>("ClientName")
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreationTime");

                    b.Property<byte>("Result");

                    b.Property<string>("TenancyName")
                        .HasMaxLength(64);

                    b.Property<int?>("TenantId");

                    b.Property<long?>("UserId");

                    b.Property<string>("UserNameOrEmailAddress")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("UserId", "TenantId");

                    b.HasIndex("TenancyName", "UserNameOrEmailAddress", "Result");

                    b.ToTable("AbpUserLoginAttempts");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserOrganizationUnit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long>("OrganizationUnitId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId", "OrganizationUnitId");

                    b.HasIndex("TenantId", "UserId");

                    b.ToTable("AbpUserOrganizationUnits");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<int>("RoleId");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TenantId", "RoleId");

                    b.HasIndex("TenantId", "UserId");

                    b.ToTable("AbpUserRoles");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TenantId", "UserId");

                    b.ToTable("AbpUserTokens");
                });

            modelBuilder.Entity("Abp.BackgroundJobs.BackgroundJobInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<bool>("IsAbandoned");

                    b.Property<string>("JobArgs")
                        .IsRequired()
                        .HasMaxLength(1048576);

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<DateTime?>("LastTryTime");

                    b.Property<DateTime>("NextTryTime");

                    b.Property<byte>("Priority");

                    b.Property<short>("TryCount");

                    b.HasKey("Id");

                    b.HasIndex("IsAbandoned", "NextTryTime");

                    b.ToTable("AbpBackgroundJobs");
                });

            modelBuilder.Entity("Abp.Configuration.Setting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int?>("TenantId");

                    b.Property<long?>("UserId");

                    b.Property<string>("Value")
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TenantId", "Name");

                    b.ToTable("AbpSettings");
                });

            modelBuilder.Entity("Abp.Localization.ApplicationLanguage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Icon")
                        .HasMaxLength(128);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId", "Name");

                    b.ToTable("AbpLanguages");
                });

            modelBuilder.Entity("Abp.Localization.ApplicationLanguageText", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("TenantId");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(67108864);

                    b.HasKey("Id");

                    b.HasIndex("TenantId", "Source", "LanguageName", "Key");

                    b.ToTable("AbpLanguageTexts");
                });

            modelBuilder.Entity("Abp.Notifications.NotificationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Data")
                        .HasMaxLength(1048576);

                    b.Property<string>("DataTypeName")
                        .HasMaxLength(512);

                    b.Property<string>("EntityId")
                        .HasMaxLength(96);

                    b.Property<string>("EntityTypeAssemblyQualifiedName")
                        .HasMaxLength(512);

                    b.Property<string>("EntityTypeName")
                        .HasMaxLength(250);

                    b.Property<string>("ExcludedUserIds")
                        .HasMaxLength(131072);

                    b.Property<string>("NotificationName")
                        .IsRequired()
                        .HasMaxLength(96);

                    b.Property<byte>("Severity");

                    b.Property<string>("TenantIds")
                        .HasMaxLength(131072);

                    b.Property<string>("UserIds")
                        .HasMaxLength(131072);

                    b.HasKey("Id");

                    b.ToTable("AbpNotifications");
                });

            modelBuilder.Entity("Abp.Notifications.NotificationSubscriptionInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("EntityId")
                        .HasMaxLength(96);

                    b.Property<string>("EntityTypeAssemblyQualifiedName")
                        .HasMaxLength(512);

                    b.Property<string>("EntityTypeName")
                        .HasMaxLength(250);

                    b.Property<string>("NotificationName")
                        .HasMaxLength(96);

                    b.Property<int?>("TenantId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NotificationName", "EntityTypeName", "EntityId", "UserId");

                    b.HasIndex("TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId");

                    b.ToTable("AbpNotificationSubscriptions");
                });

            modelBuilder.Entity("Abp.Notifications.TenantNotificationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<string>("Data")
                        .HasMaxLength(1048576);

                    b.Property<string>("DataTypeName")
                        .HasMaxLength(512);

                    b.Property<string>("EntityId")
                        .HasMaxLength(96);

                    b.Property<string>("EntityTypeAssemblyQualifiedName")
                        .HasMaxLength(512);

                    b.Property<string>("EntityTypeName")
                        .HasMaxLength(250);

                    b.Property<string>("NotificationName")
                        .IsRequired()
                        .HasMaxLength(96);

                    b.Property<byte>("Severity");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("AbpTenantNotifications");
                });

            modelBuilder.Entity("Abp.Notifications.UserNotificationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<int>("State");

                    b.Property<int?>("TenantId");

                    b.Property<Guid>("TenantNotificationId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "State", "CreationTime");

                    b.ToTable("AbpUserNotifications");
                });

            modelBuilder.Entity("Abp.Organizations.OrganizationUnit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(95);

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<long?>("ParentId");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("TenantId", "Code");

                    b.ToTable("AbpOrganizationUnits");
                });

            modelBuilder.Entity("Tbs.Authorization.Roles.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<bool>("IsDefault");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsStatic");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("DeleterUserId");

                    b.HasIndex("LastModifierUserId");

                    b.HasIndex("TenantId", "NormalizedName");

                    b.ToTable("AbpRoles");
                });

            modelBuilder.Entity("Tbs.Authorization.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AuthenticationSource")
                        .HasMaxLength(64);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<bool>("DepotSide");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("EmailConfirmationCode")
                        .HasMaxLength(328);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailConfirmed");

                    b.Property<bool>("IsLockoutEnabled");

                    b.Property<bool>("IsPhoneNumberConfirmed");

                    b.Property<bool>("IsTwoFactorEnabled");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<DateTime?>("LockoutEndDateUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("NormalizedEmailAddress")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("OperatePassword")
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("PasswordResetCode")
                        .HasMaxLength(328);

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("RoleName")
                        .HasMaxLength(32);

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int?>("TenantId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("WhName")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("DeleterUserId");

                    b.HasIndex("LastModifierUserId");

                    b.HasIndex("TenantId", "NormalizedEmailAddress");

                    b.HasIndex("TenantId", "NormalizedUserName");

                    b.ToTable("AbpUsers");
                });

            modelBuilder.Entity("Tbs.DomainModels.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArticleRecordId");

                    b.Property<int?>("ArticleRecordId1");

                    b.Property<int>("ArticleTypeId");

                    b.Property<string>("BindInfo")
                        .HasMaxLength(20);

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int>("DepotId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Rfid")
                        .HasMaxLength(20);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("ArticleRecordId1");

                    b.HasIndex("ArticleTypeId");

                    b.HasIndex("DepotId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Tbs.DomainModels.ArticleRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArticleId");

                    b.Property<int>("DepotId");

                    b.Property<DateTime>("LendTime");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ReturnTime");

                    b.Property<int>("TenantId");

                    b.Property<string>("WorkerCn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("WorkerId");

                    b.Property<string>("WorkerName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "DepotId", "ArticleId");

                    b.ToTable("ArticleRecords");
                });

            modelBuilder.Entity("Tbs.DomainModels.ArticleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BindTo")
                        .HasMaxLength(1);

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("ArticleTypes");
                });

            modelBuilder.Entity("Tbs.DomainModels.DaySettle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CarryoutDate");

                    b.Property<int>("DepotId");

                    b.Property<string>("Message")
                        .HasMaxLength(200);

                    b.Property<DateTime>("OperateTime");

                    b.Property<int?>("RoutesCount");

                    b.Property<int>("TenantId");

                    b.Property<int?>("VtAffairsCount");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "DepotId", "CarryoutDate");

                    b.ToTable("DaySettles");
                });

            modelBuilder.Entity("Tbs.DomainModels.Depot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<double?>("Latitude");

                    b.Property<double?>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("TenantId");

                    b.Property<bool>("UseRouteForIdentify");

                    b.HasKey("Id");

                    b.ToTable("Depots");
                });

            modelBuilder.Entity("Tbs.DomainModels.DepotFeature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepotId");

                    b.Property<double?>("Latitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("DepotFeatures");
                });

            modelBuilder.Entity("Tbs.DomainModels.DepotSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepotId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("DepotSettings");
                });

            modelBuilder.Entity("Tbs.DomainModels.DepotSignin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepotId");

                    b.Property<string>("EndTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("LateTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.ToTable("DepotSignins");
                });

            modelBuilder.Entity("Tbs.DomainModels.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("Mobile")
                        .HasMaxLength(11);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("Tbs.DomainModels.Outlet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ciphertext")
                        .HasMaxLength(10);

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int?>("CustomerId");

                    b.Property<int?>("DepotId");

                    b.Property<double?>("Latitude");

                    b.Property<double?>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .HasMaxLength(10);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "Cn")
                        .IsUnique();

                    b.ToTable("Outlets");
                });

            modelBuilder.Entity("Tbs.DomainModels.PreRoute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepotId");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<string>("ReturnTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("RouteCn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("RouteName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("RouteTypeId");

                    b.Property<string>("SetoutTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<int>("TenantId");

                    b.Property<int?>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("VehicleId");

                    b.HasIndex("TenantId", "DepotId", "RouteTypeId");

                    b.ToTable("PreRoutes");
                });

            modelBuilder.Entity("Tbs.DomainModels.PreRouteTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArriveTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<int>("OutletId");

                    b.Property<int>("PreRouteId");

                    b.Property<int>("TaskTypeId");

                    b.HasKey("Id");

                    b.HasIndex("OutletId");

                    b.HasIndex("PreRouteId");

                    b.HasIndex("TaskTypeId");

                    b.ToTable("PreRouteTasks");
                });

            modelBuilder.Entity("Tbs.DomainModels.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CarryoutDate");

                    b.Property<int>("DepotId");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<string>("ReturnTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("RouteCn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("RouteName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("RouteTypeId");

                    b.Property<string>("SetoutTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<int>("TenantId");

                    b.Property<int?>("VehicleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "DepotId", "CarryoutDate");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteIdentify", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("IdentifyTime");

                    b.Property<int>("OutletId");

                    b.Property<int>("RouteId");

                    b.HasKey("Id");

                    b.HasIndex("OutletId");

                    b.ToTable("RouteIdentifies");
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("Message")
                        .HasMaxLength(200);

                    b.Property<DateTime>("OperateTime");

                    b.Property<int>("RouteId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.ToTable("RouteLogs");
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArticleTypeList")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("PeerGroupNo");

                    b.Property<bool>("Required");

                    b.Property<int>("RouteTypeId");

                    b.HasKey("Id");

                    b.HasIndex("RouteTypeId");

                    b.ToTable("RouteRoles");
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArriveTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<DateTime?>("IdentifyTime");

                    b.Property<int>("OutletId");

                    b.Property<int?>("Qtum");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<int>("RouteId");

                    b.Property<int>("TaskTypeId");

                    b.HasKey("Id");

                    b.HasIndex("OutletId");

                    b.HasIndex("RouteId");

                    b.HasIndex("TaskTypeId");

                    b.ToTable("RouteTasks");
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivateAhead");

                    b.Property<int>("ArticleAhead");

                    b.Property<int>("ArticleDeadline");

                    b.Property<string>("EarliestTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("IdentifyRoleName")
                        .HasMaxLength(8);

                    b.Property<string>("LatestTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<bool>("NeedApproval");

                    b.Property<string>("SetoutRoleName")
                        .HasMaxLength(8);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("RouteTypes");
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("LendTime");

                    b.Property<string>("RecordList")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ReturnTime");

                    b.Property<int>("RouteId");

                    b.Property<int>("RouteRoleId");

                    b.Property<string>("RouteRoleName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("WorkerCn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("WorkerId");

                    b.Property<string>("WorkerName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("RouteWorkers");
                });

            modelBuilder.Entity("Tbs.DomainModels.Signin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CarryoutDate");

                    b.Property<int>("DepotId");

                    b.Property<int>("LateDistance");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<DateTime>("SigninTime");

                    b.Property<int>("TenantId");

                    b.Property<string>("Worker")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "DepotId", "CarryoutDate", "Name");

                    b.ToTable("Signins");
                });

            modelBuilder.Entity("Tbs.DomainModels.TaskType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BasicPrice");

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<string>("GroupName")
                        .HasMaxLength(8);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("TaskTypes");
                });

            modelBuilder.Entity("Tbs.DomainModels.Vault", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepotId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("TenantId");

                    b.Property<int>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.ToTable("Vaults");
                });

            modelBuilder.Entity("Tbs.DomainModels.VaultRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<bool>("Required");

                    b.Property<bool>("Single");

                    b.Property<int>("VaultTypeId");

                    b.HasKey("Id");

                    b.HasIndex("VaultTypeId");

                    b.ToTable("VaultRoles");
                });

            modelBuilder.Entity("Tbs.DomainModels.VaultType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivateAhead");

                    b.Property<string>("EarliestTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("LatestTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<bool>("NeedApproval");

                    b.Property<int>("TenantId");

                    b.Property<int>("VerifyAhead");

                    b.Property<int>("VerifyDeadline");

                    b.HasKey("Id");

                    b.ToTable("VaultTypes");
                });

            modelBuilder.Entity("Tbs.DomainModels.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("DepotId");

                    b.Property<string>("License")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.Property<int?>("MainWorkerId");

                    b.Property<byte[]>("Photo");

                    b.Property<int?>("SubWorkerId");

                    b.Property<int>("TenantId");

                    b.Property<int?>("Worker1Id");

                    b.Property<int?>("Worker2Id");

                    b.Property<int?>("Worker3Id");

                    b.Property<int?>("Worker4Id");

                    b.Property<int?>("Worker5Id");

                    b.Property<int?>("Worker6Id");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "Cn")
                        .IsUnique();

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Tbs.DomainModels.VtAffair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CarryoutDate");

                    b.Property<int>("DepotId");

                    b.Property<string>("EndTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<DateTime>("LastActiveTime");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<int>("TenantId");

                    b.Property<int>("VaultTypeId");

                    b.Property<string>("VtName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "DepotId", "CarryoutDate");

                    b.ToTable("VtAffairs");
                });

            modelBuilder.Entity("Tbs.DomainModels.VtAffairWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CheckIn");

                    b.Property<DateTime?>("CheckOut");

                    b.Property<int>("VaultRoleId");

                    b.Property<string>("VaultRoleName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("VtAffairId");

                    b.Property<string>("WorkerCn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("WorkerId");

                    b.Property<string>("WorkerName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("VtAffairId");

                    b.ToTable("VtAffairWorkers");
                });

            modelBuilder.Entity("Tbs.DomainModels.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArticleTypeList")
                        .HasMaxLength(50);

                    b.Property<int>("DepotId");

                    b.Property<string>("DepotList")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("ShiftList")
                        .HasMaxLength(50);

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Tbs.DomainModels.WhAffair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CarryoutDate");

                    b.Property<int>("DepotId");

                    b.Property<string>("EndTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<DateTime>("LastActiveTime");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<int>("TenantId");

                    b.Property<string>("WhName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "DepotId", "CarryoutDate");

                    b.ToTable("WhAffairs");
                });

            modelBuilder.Entity("Tbs.DomainModels.WhAffairWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CheckIn");

                    b.Property<DateTime?>("CheckOut");

                    b.Property<int>("WhAffairId");

                    b.Property<string>("WorkerCn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("WorkerId");

                    b.Property<string>("WorkerName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("WhAffairId");

                    b.ToTable("WhAffairWorkers");
                });

            modelBuilder.Entity("Tbs.DomainModels.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("DepotId");

                    b.Property<string>("DeviceId")
                        .HasMaxLength(50);

                    b.Property<string>("Finger")
                        .HasMaxLength(1024);

                    b.Property<string>("IDCardNo")
                        .HasMaxLength(18);

                    b.Property<string>("IDNumber")
                        .HasMaxLength(18);

                    b.Property<string>("Mobile")
                        .HasMaxLength(11);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("Password")
                        .HasMaxLength(10);

                    b.Property<byte[]>("Photo");

                    b.Property<int>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("DepotId");

                    b.HasIndex("TenantId", "Cn")
                        .IsUnique();

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Tbs.MultiTenancy.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConnectionString")
                        .HasMaxLength(1024);

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<long?>("DeleterUserId");

                    b.Property<DateTime?>("DeletionTime");

                    b.Property<int?>("EditionId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("TenancyName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("DeleterUserId");

                    b.HasIndex("EditionId");

                    b.HasIndex("LastModifierUserId");

                    b.HasIndex("TenancyName");

                    b.ToTable("AbpTenants");
                });

            modelBuilder.Entity("Abp.Application.Features.EditionFeatureSetting", b =>
                {
                    b.HasBaseType("Abp.Application.Features.FeatureSetting");

                    b.Property<int>("EditionId");

                    b.HasIndex("EditionId", "Name");

                    b.ToTable("AbpFeatures");

                    b.HasDiscriminator().HasValue("EditionFeatureSetting");
                });

            modelBuilder.Entity("Abp.MultiTenancy.TenantFeatureSetting", b =>
                {
                    b.HasBaseType("Abp.Application.Features.FeatureSetting");

                    b.Property<int>("TenantId");

                    b.HasIndex("TenantId", "Name");

                    b.ToTable("AbpFeatures");

                    b.HasDiscriminator().HasValue("TenantFeatureSetting");
                });

            modelBuilder.Entity("Abp.Authorization.Roles.RolePermissionSetting", b =>
                {
                    b.HasBaseType("Abp.Authorization.PermissionSetting");

                    b.Property<int>("RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AbpPermissions");

                    b.HasDiscriminator().HasValue("RolePermissionSetting");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserPermissionSetting", b =>
                {
                    b.HasBaseType("Abp.Authorization.PermissionSetting");

                    b.Property<long>("UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AbpPermissions");

                    b.HasDiscriminator().HasValue("UserPermissionSetting");
                });

            modelBuilder.Entity("Abp.Authorization.Roles.RoleClaim", b =>
                {
                    b.HasOne("Tbs.Authorization.Roles.Role")
                        .WithMany("Claims")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserClaim", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserLogin", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserRole", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserToken", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Abp.Configuration.Setting", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User")
                        .WithMany("Settings")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Abp.Organizations.OrganizationUnit", b =>
                {
                    b.HasOne("Abp.Organizations.OrganizationUnit", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Tbs.Authorization.Roles.Role", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("Tbs.Authorization.Users.User", "DeleterUser")
                        .WithMany()
                        .HasForeignKey("DeleterUserId");

                    b.HasOne("Tbs.Authorization.Users.User", "LastModifierUser")
                        .WithMany()
                        .HasForeignKey("LastModifierUserId");
                });

            modelBuilder.Entity("Tbs.Authorization.Users.User", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("Tbs.Authorization.Users.User", "DeleterUser")
                        .WithMany()
                        .HasForeignKey("DeleterUserId");

                    b.HasOne("Tbs.Authorization.Users.User", "LastModifierUser")
                        .WithMany()
                        .HasForeignKey("LastModifierUserId");
                });

            modelBuilder.Entity("Tbs.DomainModels.Article", b =>
                {
                    b.HasOne("Tbs.DomainModels.ArticleRecord", "ArticleRecord")
                        .WithMany()
                        .HasForeignKey("ArticleRecordId1");

                    b.HasOne("Tbs.DomainModels.ArticleType", "ArticleType")
                        .WithMany()
                        .HasForeignKey("ArticleTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.ArticleRecord", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.DaySettle", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.DepotSignin", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Outlet", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId");
                });

            modelBuilder.Entity("Tbs.DomainModels.PreRoute", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tbs.DomainModels.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("Tbs.DomainModels.PreRouteTask", b =>
                {
                    b.HasOne("Tbs.DomainModels.Outlet", "Outlet")
                        .WithMany()
                        .HasForeignKey("OutletId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tbs.DomainModels.PreRoute")
                        .WithMany("Tasks")
                        .HasForeignKey("PreRouteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tbs.DomainModels.TaskType", "TaskType")
                        .WithMany()
                        .HasForeignKey("TaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Route", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteIdentify", b =>
                {
                    b.HasOne("Tbs.DomainModels.Outlet", "Outlet")
                        .WithMany()
                        .HasForeignKey("OutletId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteRole", b =>
                {
                    b.HasOne("Tbs.DomainModels.RouteType")
                        .WithMany("Roles")
                        .HasForeignKey("RouteTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteTask", b =>
                {
                    b.HasOne("Tbs.DomainModels.Outlet", "Outlet")
                        .WithMany()
                        .HasForeignKey("OutletId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tbs.DomainModels.Route")
                        .WithMany("Tasks")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tbs.DomainModels.TaskType", "TaskType")
                        .WithMany()
                        .HasForeignKey("TaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.RouteWorker", b =>
                {
                    b.HasOne("Tbs.DomainModels.Route")
                        .WithMany("Workers")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Signin", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Vault", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.VaultRole", b =>
                {
                    b.HasOne("Tbs.DomainModels.VaultType")
                        .WithMany("Roles")
                        .HasForeignKey("VaultTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Vehicle", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.VtAffair", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.VtAffairWorker", b =>
                {
                    b.HasOne("Tbs.DomainModels.VtAffair")
                        .WithMany("Workers")
                        .HasForeignKey("VtAffairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Warehouse", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.WhAffair", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.WhAffairWorker", b =>
                {
                    b.HasOne("Tbs.DomainModels.WhAffair")
                        .WithMany("Workers")
                        .HasForeignKey("WhAffairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.DomainModels.Worker", b =>
                {
                    b.HasOne("Tbs.DomainModels.Depot", "Depot")
                        .WithMany()
                        .HasForeignKey("DepotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tbs.MultiTenancy.Tenant", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("Tbs.Authorization.Users.User", "DeleterUser")
                        .WithMany()
                        .HasForeignKey("DeleterUserId");

                    b.HasOne("Abp.Application.Editions.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId");

                    b.HasOne("Tbs.Authorization.Users.User", "LastModifierUser")
                        .WithMany()
                        .HasForeignKey("LastModifierUserId");
                });

            modelBuilder.Entity("Abp.Application.Features.EditionFeatureSetting", b =>
                {
                    b.HasOne("Abp.Application.Editions.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Abp.Authorization.Roles.RolePermissionSetting", b =>
                {
                    b.HasOne("Tbs.Authorization.Roles.Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Abp.Authorization.Users.UserPermissionSetting", b =>
                {
                    b.HasOne("Tbs.Authorization.Users.User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
