using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.EventManager.Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_Company_CompanyId",
                table: "CompanyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_User_UserId",
                table: "CompanyUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUser",
                table: "CompanyUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_UserId",
                table: "CompanyUser");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyUser");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanyUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CompanyUser",
                newName: "CompaniesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUser",
                table: "CompanyUser",
                columns: new[] { "CompaniesId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UsersId",
                table: "CompanyUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_Company_CompaniesId",
                table: "CompanyUser",
                column: "CompaniesId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_User_UsersId",
                table: "CompanyUser",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_Company_CompaniesId",
                table: "CompanyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUser_User_UsersId",
                table: "CompanyUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUser",
                table: "CompanyUser");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUser_UsersId",
                table: "CompanyUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "CompanyUser",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CompaniesId",
                table: "CompanyUser",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUser",
                table: "CompanyUser",
                columns: new[] { "CompanyId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUser_UserId",
                table: "CompanyUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_Company_CompanyId",
                table: "CompanyUser",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUser_User_UserId",
                table: "CompanyUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
