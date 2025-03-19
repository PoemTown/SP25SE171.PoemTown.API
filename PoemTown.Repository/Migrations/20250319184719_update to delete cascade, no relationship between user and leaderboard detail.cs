using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatetodeletecascadenorelationshipbetweenuserandleaderboarddetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderBoardDetails_LeaderBoards_LeaderBoardId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderBoardDetails_Poems_PoemId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderBoardDetails_Users_UserId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropIndex(
                name: "IX_LeaderBoardDetails_PoemId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropIndex(
                name: "IX_LeaderBoardDetails_UserId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LeaderBoardDetails");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4891), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4891), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4914), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4914), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4787), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4788), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4778), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4779), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4826), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4826), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4832), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4832), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "a94979c0-6a6a-46cc-916c-7a50f72beff6", new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4557), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4557), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "d23e39e4-0f0d-41db-9eb6-0c228429dd09", new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4625), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 1, 47, 18, 544, DateTimeKind.Unspecified).AddTicks(4625), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderBoardDetails_PoemId",
                table: "LeaderBoardDetails",
                column: "PoemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderBoardDetails_LeaderBoards_LeaderBoardId",
                table: "LeaderBoardDetails",
                column: "LeaderBoardId",
                principalTable: "LeaderBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderBoardDetails_Poems_PoemId",
                table: "LeaderBoardDetails",
                column: "PoemId",
                principalTable: "Poems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaderBoardDetails_LeaderBoards_LeaderBoardId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaderBoardDetails_Poems_PoemId",
                table: "LeaderBoardDetails");

            migrationBuilder.DropIndex(
                name: "IX_LeaderBoardDetails_PoemId",
                table: "LeaderBoardDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "LeaderBoardDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3948), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3948), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3983), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3983), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3763), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3765), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3752), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3754), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3841), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3841), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3851), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3851), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6b7f1a34-5241-4c06-ad48-95a70bd220b9", new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3418), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3418), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "04f05cf9-26c6-4076-bfd7-1d1297f5f2af", new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3510), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 0, 33, 57, 280, DateTimeKind.Unspecified).AddTicks(3510), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderBoardDetails_PoemId",
                table: "LeaderBoardDetails",
                column: "PoemId",
                unique: true,
                filter: "[PoemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderBoardDetails_UserId",
                table: "LeaderBoardDetails",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderBoardDetails_LeaderBoards_LeaderBoardId",
                table: "LeaderBoardDetails",
                column: "LeaderBoardId",
                principalTable: "LeaderBoards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderBoardDetails_Poems_PoemId",
                table: "LeaderBoardDetails",
                column: "PoemId",
                principalTable: "Poems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaderBoardDetails_Users_UserId",
                table: "LeaderBoardDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
