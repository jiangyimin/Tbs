using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tbs.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeixinCorpId",
                table: "AbpTenants");

            migrationBuilder.AddColumn<bool>(
                name: "UseRouteForIdentify",
                table: "Depots",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WhName",
                table: "AbpUsers",
                maxLength: 8,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DaySettle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarryoutDate = table.Column<DateTime>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 200, nullable: true),
                    OperateTime = table.Column<DateTime>(nullable: false),
                    RoutesCount = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    VtAffairsCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaySettle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaySettle_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaySettle_DepotId",
                table: "DaySettle",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_DaySettle_TenantId_DepotId_CarryoutDate",
                table: "DaySettle",
                columns: new[] { "TenantId", "DepotId", "CarryoutDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaySettle");

            migrationBuilder.DropColumn(
                name: "UseRouteForIdentify",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "WhName",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "WeixinCorpId",
                table: "AbpTenants",
                maxLength: 30,
                nullable: true);
        }
    }
}
