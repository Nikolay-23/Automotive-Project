using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automotive_Project.Migrations
{
    /// <inheritdoc />
    public partial class _43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 19, 9, 47, 650, DateTimeKind.Local).AddTicks(4066));
        }
    }
}
