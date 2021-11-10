using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class updatedAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "woodEmployee",
                table: "AspNetUsers",
                newName: "IsWoodEmployee");

            migrationBuilder.RenameColumn(
                name: "clientName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "clientName");

            migrationBuilder.RenameColumn(
                name: "IsWoodEmployee",
                table: "AspNetUsers",
                newName: "woodEmployee");
        }
    }
}
