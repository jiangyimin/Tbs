using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tbs.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaySettle_Depots_DepotId",
                table: "DaySettle");

            migrationBuilder.DropTable(
                name: "PreWorkers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DaySettle",
                table: "DaySettle");

            migrationBuilder.RenameTable(
                name: "DaySettle",
                newName: "DaySettles");

            migrationBuilder.RenameIndex(
                name: "IX_DaySettle_TenantId_DepotId_CarryoutDate",
                table: "DaySettles",
                newName: "IX_DaySettles_TenantId_DepotId_CarryoutDate");

            migrationBuilder.RenameIndex(
                name: "IX_DaySettle_DepotId",
                table: "DaySettles",
                newName: "IX_DaySettles_DepotId");

            migrationBuilder.AddColumn<int>(
                name: "Worker1Id",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Worker2Id",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Worker3Id",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Worker4Id",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Worker5Id",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Worker6Id",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BindInfo",
                table: "Articles",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DaySettles",
                table: "DaySettles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DaySettles_Depots_DepotId",
                table: "DaySettles",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaySettles_Depots_DepotId",
                table: "DaySettles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DaySettles",
                table: "DaySettles");

            migrationBuilder.DropColumn(
                name: "Worker1Id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Worker2Id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Worker3Id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Worker4Id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Worker5Id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Worker6Id",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "DaySettles",
                newName: "DaySettle");

            migrationBuilder.RenameIndex(
                name: "IX_DaySettles_TenantId_DepotId_CarryoutDate",
                table: "DaySettle",
                newName: "IX_DaySettle_TenantId_DepotId_CarryoutDate");

            migrationBuilder.RenameIndex(
                name: "IX_DaySettles_DepotId",
                table: "DaySettle",
                newName: "IX_DaySettle_DepotId");

            migrationBuilder.AlterColumn<string>(
                name: "BindInfo",
                table: "Articles",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DaySettle",
                table: "DaySettle",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DaySettle_Depots_DepotId",
                table: "DaySettle",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
