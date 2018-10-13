using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tbs.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperatePassword",
                table: "AbpUsers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PreWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepotId = table.Column<int>(nullable: false),
                    RouteCn = table.Column<string>(maxLength: 8, nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Worker1Id = table.Column<int>(nullable: true),
                    Worker2Id = table.Column<int>(nullable: true),
                    Worker3Id = table.Column<int>(nullable: true),
                    Worker4Id = table.Column<int>(nullable: true),
                    Worker5Id = table.Column<int>(nullable: true),
                    Worker6Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreWorkers_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreWorkers_DepotId",
                table: "PreWorkers",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_PreWorkers_TenantId_DepotId_RouteCn",
                table: "PreWorkers",
                columns: new[] { "TenantId", "DepotId", "RouteCn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreWorkers");

            migrationBuilder.DropColumn(
                name: "OperatePassword",
                table: "AbpUsers");
        }
    }
}
