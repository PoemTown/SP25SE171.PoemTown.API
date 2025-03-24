using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatedbforleaderboardandleaderboarddetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "LeaderBoards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LeaderBoards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LeaderBoardDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "LeaderBoards",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "EndDate", "LastUpdatedBy", "LastUpdatedTime", "StartDate", "Status", "Type" },
                values: new object[,]
                {
                    { new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"), null, new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 653, DateTimeKind.Unspecified).AddTicks(8958), new TimeSpan(0, 7, 0, 0, 0)), null, null, new DateTime(2025, 3, 19, 11, 32, 39, 653, DateTimeKind.Utc).AddTicks(9972), null, new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 653, DateTimeKind.Unspecified).AddTicks(8958), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 11, 32, 39, 653, DateTimeKind.Utc).AddTicks(9683), 0, 0 },
                    { new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"), null, new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 654, DateTimeKind.Unspecified).AddTicks(276), new TimeSpan(0, 7, 0, 0, 0)), null, null, new DateTime(2025, 3, 19, 11, 32, 39, 654, DateTimeKind.Utc).AddTicks(285), null, new DateTimeOffset(new DateTime(2025, 3, 19, 18, 32, 39, 654, DateTimeKind.Unspecified).AddTicks(276), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(2025, 3, 19, 11, 32, 39, 654, DateTimeKind.Utc).AddTicks(284), 0, 1 }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"));

            migrationBuilder.DeleteData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LeaderBoards");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LeaderBoardDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "LeaderBoardId",
                table: "UserLeaderBoards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "LeaderBoards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5640), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5642), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5626), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5628), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5825), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5825), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5846), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5846), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "59d93aab-ff6b-46a7-a768-3053652a32ce", new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5316), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5316), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "a5e04c47-48ff-4ccd-9723-cf9731df5741", new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5447), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 13, 21, 47, 15, 909, DateTimeKind.Unspecified).AddTicks(5447), new TimeSpan(0, 7, 0, 0, 0)) });

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
    }
}
