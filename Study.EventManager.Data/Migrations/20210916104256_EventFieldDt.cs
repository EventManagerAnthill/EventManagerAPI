using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.EventManager.Data.Migrations
{
    public partial class EventFieldDt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BeginHoldingDate",
                table: "Event",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "CompanySubscription",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginHoldingDate",
                table: "Event");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "CompanySubscription",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
