using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addingAppUserOfficeRolesUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles",
                columns: new[] { "AppUserId", "RoleId", "ImtsOfficeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserOfficeRoles",
                table: "AppUserOfficeRoles",
                columns: new[] { "AppUserId", "RoleId" });
        }
    }
}
