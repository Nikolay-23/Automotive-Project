using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automotive_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleUserAccount_Roles_RolesId",
                table: "AppRoleUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleUserAccount_UserAccounts_UsersId",
                table: "AppRoleUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ApplicationUsers_ClientId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserAccounts_UserAccountId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRoleUserAccount",
                table: "AppRoleUserAccount");

            migrationBuilder.RenameTable(
                name: "AppRoleUserAccount",
                newName: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "Orders",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserAccountId",
                table: "Orders",
                newName: "IX_Orders_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppRoleUserAccount_UsersId",
                table: "UserRoles",
                newName: "IX_UserRoles_UsersId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ApplicationUsers_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserAccounts_ClientId1",
                table: "Orders",
                column: "ClientId1",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_UserAccounts_UsersId",
                table: "UserRoles",
                column: "UsersId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ApplicationUsers_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserAccounts_ClientId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_UserAccounts_UsersId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "AppRoleUserAccount");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Orders",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                newName: "IX_Orders_UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UsersId",
                table: "AppRoleUserAccount",
                newName: "IX_AppRoleUserAccount_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRoleUserAccount",
                table: "AppRoleUserAccount",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleUserAccount_Roles_RolesId",
                table: "AppRoleUserAccount",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleUserAccount_UserAccounts_UsersId",
                table: "AppRoleUserAccount",
                column: "UsersId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ApplicationUsers_ClientId1",
                table: "Orders",
                column: "ClientId1",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserAccounts_UserAccountId",
                table: "Orders",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id");
        }
    }
}
