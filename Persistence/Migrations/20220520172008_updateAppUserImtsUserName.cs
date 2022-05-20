using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateAppUserImtsUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImtsUserName",
                table: "AspNetUsers",
                newName: "ImtsEmployeeUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImtsEmployeeUserName",
                table: "AspNetUsers",
                newName: "ImtsUserName");
        }
    }
}
