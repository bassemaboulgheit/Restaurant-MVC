using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastResetDate",
                value: new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
