using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.EventManager.Data.Migrations
{
    public partial class fifthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_CompanyId",
                table: "CompanyUser",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UserId",
                table: "CompanyUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_Company_CompanyId",
                table: "CompanyUser",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_User_UserId",
                table: "CompanyUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_Company_CompanyId",
                table: "CompanyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_User_UserId",
                table: "CompanyUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_CompanyId",
                table: "CompanyUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_UserId",
                table: "CompanyUser");
        }
    }
}
