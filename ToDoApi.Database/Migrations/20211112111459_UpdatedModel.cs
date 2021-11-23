using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace TodoAPI.Migrations
{
    public partial class UpdatedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TodoItems",
                keyColumn: "ItemID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TodoLists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "TodoItems");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TodoItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "todoListsId",
                table: "TodoItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_todoListsId",
                table: "TodoItems",
                column: "todoListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoLists_todoListsId",
                table: "TodoItems",
                column: "todoListsId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoLists_todoListsId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_todoListsId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "todoListsId",
                table: "TodoItems");

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "TodoItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "ItemID", "CreatedBy", "CreatedDateTime", "ItemGuid", "ItemName", "ListId", "ModifiedDateTime" },
                values: new object[] { 1, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2021, 11, 11, 20, 32, 39, 628, DateTimeKind.Local).AddTicks(2901), new Guid("00000000-0000-0000-0000-000000000000"), "TestItem", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "TodoLists",
                columns: new[] { "Id", "CreatedBy", "CreatedDateTime", "ListGuid", "ModifiedDateTime", "TodoListName" },
                values: new object[] { 1, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2021, 11, 11, 20, 32, 39, 625, DateTimeKind.Local).AddTicks(8026), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test" });
        }
    }
}
