using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class userhasmanyrecordfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RecordFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Poems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(5030), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(5030), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(5054), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(5054), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4864), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4866), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4852), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4853), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4941), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4941), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4948), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4948), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "dac82eab-c364-4f58-818c-b4f741521365", new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4513), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4513), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "57d683c5-86fa-49cd-87b0-3100fbc7a81f", new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4621), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 17, 20, 13, 591, DateTimeKind.Unspecified).AddTicks(4621), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_RecordFiles_UserId",
                table: "RecordFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Poems_UserId",
                table: "Poems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Poems_Users_UserId",
                table: "Poems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordFiles_Users_UserId",
                table: "RecordFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poems_Users_UserId",
                table: "Poems");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordFiles_Users_UserId",
                table: "RecordFiles");

            migrationBuilder.DropIndex(
                name: "IX_RecordFiles_UserId",
                table: "RecordFiles");

            migrationBuilder.DropIndex(
                name: "IX_Poems_UserId",
                table: "Poems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Poems");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6691), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6691), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6706), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6706), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6604), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6605), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6597), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6598), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6638), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6638), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6643), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6643), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "2be4aeca-c27c-4ee3-befb-906ddea733c0", new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6481), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6481), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "45b7dbf4-b21a-4144-9207-a750b81b4afe", new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6539), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 14, 44, 11, 911, DateTimeKind.Unspecified).AddTicks(6539), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
