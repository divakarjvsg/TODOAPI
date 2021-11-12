using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoAPI.Migrations
{
    public partial class assignLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoLists_TodoListId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_TodoListId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "TodoListId",
                table: "TodoItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "TodoLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ListGuid",
                table: "TodoLists",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "TodoLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "TodoItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemGuid",
                table: "TodoItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "TodoItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AssignLabels",
                columns: table => new
                {
                    AssignId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelId = table.Column<int>(nullable: false),
                    AssignedGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignLabels", x => x.AssignId);
                });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "ItemID",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2021, 11, 11, 10, 21, 43, 732, DateTimeKind.Local).AddTicks(7256));

            migrationBuilder.UpdateData(
                table: "TodoLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2021, 11, 11, 10, 21, 43, 730, DateTimeKind.Local).AddTicks(7901));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignLabels");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "ListGuid",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ItemGuid",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "TodoItems");

            migrationBuilder.AddColumn<int>(
                name: "TodoListId",
                table: "TodoItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_TodoListId",
                table: "TodoItems",
                column: "TodoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoLists_TodoListId",
                table: "TodoItems",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
