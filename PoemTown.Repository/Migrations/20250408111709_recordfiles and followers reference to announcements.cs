using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class recordfilesandfollowersreferencetoannouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FollowerId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecordFileId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4493), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4493), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4525), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4525), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4328), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4330), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4311), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4312), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4391), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4391), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4408), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4408), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6a6f5aff-4b79-4ee5-9634-fd9cf74b3076", new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4104), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4104), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "cbf29e1d-03e0-466d-8996-81a5ad567bb9", new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4197), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 18, 17, 8, 368, DateTimeKind.Unspecified).AddTicks(4197), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_FollowerId",
                table: "Announcements",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_RecordFileId",
                table: "Announcements",
                column: "RecordFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Followers_FollowerId",
                table: "Announcements",
                column: "FollowerId",
                principalTable: "Followers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_RecordFiles_RecordFileId",
                table: "Announcements",
                column: "RecordFileId",
                principalTable: "RecordFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Followers_FollowerId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_RecordFiles_RecordFileId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_FollowerId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_RecordFileId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "FollowerId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "RecordFileId",
                table: "Announcements");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1511), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1511), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1561), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1561), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1269), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1271), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1255), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1258), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1372), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1372), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1404), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1404), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "0c0f7781-70be-45d4-a01c-4612ccb27dbd", new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(964), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(964), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "0208c0d8-0863-4782-8130-5e0f5a6513e0", new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1084), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 8, 0, 59, 57, 294, DateTimeKind.Unspecified).AddTicks(1084), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
