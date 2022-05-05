using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ImtsUserAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "officeId",
                table: "AspNetUsers",
                newName: "MainOfficeId");

            migrationBuilder.RenameColumn(
                name: "IsWoodEmployee",
                table: "AspNetUsers",
                newName: "IsImtsUser");

            migrationBuilder.AddColumn<string>(
                name: "ImtsUserName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImtsUserName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "MainOfficeId",
                table: "AspNetUsers",
                newName: "officeId");

            migrationBuilder.RenameColumn(
                name: "IsImtsUser",
                table: "AspNetUsers",
                newName: "IsWoodEmployee");
        }
    }
}
