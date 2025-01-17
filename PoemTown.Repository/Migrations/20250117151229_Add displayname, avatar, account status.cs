using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Adddisplaynameavataraccountstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6186), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6187), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6179), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6180), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6225), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6225), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6237), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6237), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "Avatar", "ConcurrencyStamp", "CreatedTime", "DisplayName", "LastUpdatedTime", "Status" },
                values: new object[] { null, "8ac91cb7-7c46-4f58-b5c6-61cfa24a96eb", new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6024), new TimeSpan(0, 7, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6024), new TimeSpan(0, 7, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "Avatar", "ConcurrencyStamp", "CreatedTime", "DisplayName", "LastUpdatedTime", "Status" },
                values: new object[] { null, "08da8c1b-2ba6-424c-b992-df45185c72a6", new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6096), new TimeSpan(0, 7, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2025, 1, 17, 22, 12, 28, 593, DateTimeKind.Unspecified).AddTicks(6096), new TimeSpan(0, 7, 0, 0, 0)), 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9304), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9305), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9298), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9299), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9345), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9345), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9351), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9351), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "2e8c3912-e2da-47d5-b4a4-574a4a9a5442", new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9151), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9151), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "16fd21f3-0152-494e-b1ca-143acd98010a", new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9219), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 17, 21, 54, 6, 349, DateTimeKind.Unspecified).AddTicks(9219), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
