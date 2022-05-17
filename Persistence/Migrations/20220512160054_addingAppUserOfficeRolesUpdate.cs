using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addingAppUserOfficeRolesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserOfficeRoles_AspNetUsers_AppuserId",
                table: "AppUserOfficeRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles");

            migrationBuilder.RenameColumn(
                name: "AppuserId",
                table: "AppUserOfficeRoles",
                newName: "AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "ImtsOfficeId",
                table: "AppUserOfficeRoles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles",
                columns: new[] { "AppUserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserOfficeRoles_AspNetUsers_AppUserId",
                table: "AppUserOfficeRoles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserOfficeRoles_AspNetUsers_AppUserId",
                table: "AppUserOfficeRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AppUserOfficeRoles",
                newName: "AppuserId");

            migrationBuilder.AlterColumn<int>(
                name: "ImtsOfficeId",
                table: "AppUserOfficeRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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
        }
    }
}
