using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPreparationTime",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DailyOrderCount",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastResetDate",
                table: "MenuItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PreparationTime",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DailyOrderCount", "LastResetDate", "PreparationTime" },
                values: new object[] { 0, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local), 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPreparationTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DailyOrderCount",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "LastResetDate",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "MenuItems");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
