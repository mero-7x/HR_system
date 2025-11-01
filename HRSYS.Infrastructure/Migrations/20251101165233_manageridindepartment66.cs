using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSYS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class manageridindepartment66 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Leaves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Leaves",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
