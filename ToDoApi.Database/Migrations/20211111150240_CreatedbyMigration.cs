using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace TodoAPI.Migrations
{
    public partial class CreatedbyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TodoLists",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TodoItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Labels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "ItemID",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2021, 11, 11, 20, 32, 39, 628, DateTimeKind.Local).AddTicks(2901));

            migrationBuilder.UpdateData(
                table: "TodoLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2021, 11, 11, 20, 32, 39, 625, DateTimeKind.Local).AddTicks(8026));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Labels");

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "ItemID",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2021, 11, 11, 16, 34, 17, 592, DateTimeKind.Local).AddTicks(6454));

            migrationBuilder.UpdateData(
                table: "TodoLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2021, 11, 11, 16, 34, 17, 590, DateTimeKind.Local).AddTicks(619));
        }
    }
}
