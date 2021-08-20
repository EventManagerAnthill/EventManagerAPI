using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.EventManager.Data.Migrations
{
    public partial class eigththMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_User_UserId1",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_UserId1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Company");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Company",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_UserId1",
                table: "Company",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_User_UserId1",
                table: "Company",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
