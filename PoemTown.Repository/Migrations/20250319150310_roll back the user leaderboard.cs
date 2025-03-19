using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class rollbacktheuserleaderboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "LeaderBoardDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "LeaderBoardId",
                table: "UserLeaderBoards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 7, 604, DateTimeKind.Unspecified).AddTicks(6769), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 15, 3, 7, 605, DateTimeKind.Utc).AddTicks(1693), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 7, 604, DateTimeKind.Unspecified).AddTicks(6769), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 15, 3, 7, 605, DateTimeKind.Utc).AddTicks(239) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 7, 605, DateTimeKind.Unspecified).AddTicks(3215), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 15, 3, 7, 605, DateTimeKind.Utc).AddTicks(3251), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 7, 605, DateTimeKind.Unspecified).AddTicks(3215), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 15, 3, 7, 605, DateTimeKind.Utc).AddTicks(3249) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9568), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9570), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9558), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9559), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9654), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9654), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9664), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9664), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "87691d37-3192-41d0-aebe-c40d5f394c54", new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9153), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9153), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "65043bca-a47f-4db2-ba77-c85223e29344", new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9277), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 22, 3, 8, 739, DateTimeKind.Unspecified).AddTicks(9277), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_UserLeaderBoards_LeaderBoardId",
                table: "UserLeaderBoards",
                column: "LeaderBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLeaderBoards_LeaderBoards_LeaderBoardId",
                table: "UserLeaderBoards",
                column: "LeaderBoardId",
                principalTable: "LeaderBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLeaderBoards_LeaderBoards_LeaderBoardId",
                table: "UserLeaderBoards");

            migrationBuilder.DropIndex(
                name: "IX_UserLeaderBoards_LeaderBoardId",
                table: "UserLeaderBoards");

            migrationBuilder.DropColumn(
                name: "LeaderBoardId",
                table: "UserLeaderBoards");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LeaderBoardDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 653, DateTimeKind.Unspecified).AddTicks(8958), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 11, 32, 39, 653, DateTimeKind.Utc).AddTicks(9972), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 653, DateTimeKind.Unspecified).AddTicks(8958), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 11, 32, 39, 653, DateTimeKind.Utc).AddTicks(9683) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 654, DateTimeKind.Unspecified).AddTicks(276), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 11, 32, 39, 654, DateTimeKind.Utc).AddTicks(285), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 654, DateTimeKind.Unspecified).AddTicks(276), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 11, 32, 39, 654, DateTimeKind.Utc).AddTicks(284) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3850), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3851), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3843), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3844), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3888), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3888), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3893), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3893), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "e9e4fdfc-4c89-45eb-a134-94156f6b4a63", new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3730), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3730), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "ea0fb6f5-840c-4fb7-9720-2850e164ebc9", new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3782), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 40, 191, DateTimeKind.Unspecified).AddTicks(3782), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
