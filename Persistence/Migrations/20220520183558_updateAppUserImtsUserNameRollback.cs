using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateAppUserImtsUserNameRollback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImtsEmployeeUserName",
                table: "AspNetUsers",
                newName: "ImtsUserName");

            migrationBuilder.AlterColumn<int>(
                name: "ImtsEmployeeId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImtsUserName",
                table: "AspNetUsers",
                newName: "ImtsEmployeeUserName");

            migrationBuilder.AlterColumn<int>(
                name: "ImtsEmployeeId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
