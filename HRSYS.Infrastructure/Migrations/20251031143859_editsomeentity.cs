using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSYS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editsomeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Users_ApprovedManagerId",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "ApprovedManagerId",
                table: "Leaves",
                newName: "ByManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Leaves_ApprovedManagerId",
                table: "Leaves",
                newName: "IX_Leaves_ByManagerId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Leaves",
                type: "text",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerRejectDate",
                table: "Leaves",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Users_ByManagerId",
                table: "Leaves",
                column: "ByManagerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Users_ByManagerId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "ManagerRejectDate",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "ByManagerId",
                table: "Leaves",
                newName: "ApprovedManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Leaves_ByManagerId",
                table: "Leaves",
                newName: "IX_Leaves_ApprovedManagerId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Leaves",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Pending");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Users_ApprovedManagerId",
                table: "Leaves",
                column: "ApprovedManagerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
