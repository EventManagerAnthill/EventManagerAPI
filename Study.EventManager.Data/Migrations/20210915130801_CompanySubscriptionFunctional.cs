using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.EventManager.Data.Migrations
{
    public partial class CompanySubscriptionFunctional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "CompanySubscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubscriptionRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidityDays = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    isFree = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Del = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubscription_SubscriptionId",
                table: "CompanySubscription",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySubscription_SubscriptionRates_SubscriptionId",
                table: "CompanySubscription",
                column: "SubscriptionId",
                principalTable: "SubscriptionRates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DropForeignKey(
                name: "FK_CompanySubscription_SubscriptionRates_SubscriptionId",
                table: "CompanySubscription");*/

            migrationBuilder.DropTable(
                name: "SubscriptionRates");

            migrationBuilder.DropIndex(
                name: "IX_CompanySubscription_SubscriptionId",
                table: "CompanySubscription");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "CompanySubscription");
        }
    }
}
