using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automotive_Project.Migrations
{
    /// <inheritdoc />
    public partial class Initial43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Category", "CreatedAt", "Description", "ImageFileName", "Name", "Price" },
                values: new object[] { 1, "Audi", "Filters", new DateTime(2025, 9, 27, 19, 9, 47, 650, DateTimeKind.Local).AddTicks(4066), "One of the most used filters on market", "OilFIlter.jpg", "Oil Filter DENCKERMANN A210734", 123m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
