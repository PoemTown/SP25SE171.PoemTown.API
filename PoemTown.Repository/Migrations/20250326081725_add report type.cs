using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addreporttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8190), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8190), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8213), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8213), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8087), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8088), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8079), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8081), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8131), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8131), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8137), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8137), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "bc9b0805-bfa8-4b51-b0e0-c4a8b4138048", new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7936), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7936), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "aab9e630-c66f-4408-99d5-97e7088221fb", new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7998), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7998), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Reports");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8559), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8559), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8586), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8586), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8451), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8452), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8443), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8444), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8495), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8495), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8503), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8503), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "3f788232-324e-4e45-b4a3-efafd4a04762", new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8289), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8289), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "eb0a8e19-fc0b-480e-969a-d030d2f7df56", new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8361), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 48, 21, 741, DateTimeKind.Unspecified).AddTicks(8361), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
