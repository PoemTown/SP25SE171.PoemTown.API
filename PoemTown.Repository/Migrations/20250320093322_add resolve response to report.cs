using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addresolveresponsetoreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolveResponse",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6522), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6522), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6553), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6553), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6425), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6426), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6418), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6419), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6464), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6464), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6470), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6470), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "0a090b9d-80c1-4f45-8eff-359b511350af", new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6271), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6271), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "ffaadafd-db89-417c-9798-6a079b55f3d9", new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6342), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 16, 33, 21, 20, DateTimeKind.Unspecified).AddTicks(6342), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolveResponse",
                table: "Reports");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7299), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7299), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7336), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7336), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7047), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7050), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7030), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7033), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7162), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7162), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7179), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(7179), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "5c8190b0-82e0-416e-842d-a864f346bc17", new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(6676), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(6676), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "904efdc0-4fd3-4283-ae38-f09c3fdd9013", new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(6849), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 20, 15, 44, 26, 468, DateTimeKind.Unspecified).AddTicks(6849), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
