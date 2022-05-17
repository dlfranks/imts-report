using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addingAppUserOfficeRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AspNetUsers_AppuserId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_OfficeRoles_RoleId",
                table: "AppUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles");

            migrationBuilder.RenameTable(
                name: "AppUserRoles",
                newName: "AppUserOfficeRoles");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AppUserOfficeRoles",
                newName: "IX_AppUserOfficeRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles",
                columns: new[] { "AppuserId", "ImtsOfficeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserOfficeRoles_AspNetUsers_AppuserId",
                table: "AppUserOfficeRoles",
                column: "AppuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserOfficeRoles_OfficeRoles_RoleId",
                table: "AppUserOfficeRoles",
                column: "RoleId",
                principalTable: "OfficeRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserOfficeRoles_AspNetUsers_AppuserId",
                table: "AppUserOfficeRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserOfficeRoles_OfficeRoles_RoleId",
                table: "AppUserOfficeRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles");

            migrationBuilder.RenameTable(
                name: "AppUserOfficeRoles",
                newName: "AppUserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserOfficeRoles_RoleId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles",
                columns: new[] { "AppuserId", "ImtsOfficeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AspNetUsers_AppuserId",
                table: "AppUserRoles",
                column: "AppuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_OfficeRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId",
                principalTable: "OfficeRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
