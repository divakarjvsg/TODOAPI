using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoAPI.Migrations
{
    public partial class DateLabelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoLists_todoListsId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "todoListsId",
                table: "TodoItems",
                newName: "TodoListsId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoItems_todoListsId",
                table: "TodoItems",
                newName: "IX_TodoItems_TodoListsId");

            migrationBuilder.AlterColumn<string>(
                name: "TodoListName",
                table: "TodoLists",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "TodoLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "TodoItems",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "TodoItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Labels",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoLists_TodoListsId",
                table: "TodoItems",
                column: "TodoListsId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoLists_TodoListsId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "TodoListsId",
                table: "TodoItems",
                newName: "todoListsId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoItems_TodoListsId",
                table: "TodoItems",
                newName: "IX_TodoItems_todoListsId");

            migrationBuilder.AlterColumn<string>(
                name: "TodoListName",
                table: "TodoLists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "TodoLists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "TodoItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Labels",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoLists_todoListsId",
                table: "TodoItems",
                column: "todoListsId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
