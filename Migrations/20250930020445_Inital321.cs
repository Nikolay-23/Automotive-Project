using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Automotive_Project.Migrations
{
    /// <inheritdoc />
    public partial class Inital321 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 153m);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Category", "CreatedAt", "Description", "ImageFileName", "Name", "Price" },
                values: new object[,]
                {
                    { 2, "Audi", "Filters", new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filter with high efficiency", "K&N High Performance Air Filter.jpg", "K&N High Performance Air Filter", 267m },
                    { 3, "Audi", "Filters", new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fuel filter", "Mahle Fuel Filter.jpg", "Mahle Fuel Filter", 300m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 123m);
        }
    }
}
