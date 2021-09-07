using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.EventManager.Data.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "EventUserLink",
                newName: "UserEventRole");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "CompanyUserLink",
                newName: "UserCompanyRole");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserEventRole",
                table: "EventUserLink",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "UserCompanyRole",
                table: "CompanyUserLink",
                newName: "UserRole");
        }
    }
}
