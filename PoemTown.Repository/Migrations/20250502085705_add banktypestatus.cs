using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addbanktypestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BankTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9148), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9148), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9171), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9171), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9037), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9038), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9029), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9030), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9081), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9081), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9089), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9089), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6dfcf385-ccf6-4f7c-8fcb-9e5fedee67a9", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8879), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8879), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "f24d7c91-798c-4c3e-9f5d-aeb28f282237", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8952), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8952), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BankTypes");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9126), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9126), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9155), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9155), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8999), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9000), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8989), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8990), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9056), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9056), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9064), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9064), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "8d9a2f97-309e-4a6a-bc25-3263b510d745", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "4e026b7b-7186-4c04-92e2-990ee3c374da", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8903), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8903), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
