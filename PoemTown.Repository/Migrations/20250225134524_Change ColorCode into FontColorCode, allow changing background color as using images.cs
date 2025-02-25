using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColorCodeintoFontColorCodeallowchangingbackgroundcolorasusingimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesignType",
                table: "UserTemplateDetails");

            migrationBuilder.DropColumn(
                name: "DesignType",
                table: "MasterTemplateDetails");

            migrationBuilder.RenameColumn(
                name: "ColorCode",
                table: "UserTemplateDetails",
                newName: "FontColorCode");

            migrationBuilder.RenameColumn(
                name: "ColorCode",
                table: "MasterTemplateDetails",
                newName: "FontColorCode");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5562), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5565), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5547), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5550), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5656), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5656), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5674), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5674), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "4acea618-1480-4477-8f5a-4db05c4e5344", new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5231), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5231), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "251e761d-f4d7-4498-a1c3-dac50d6ee7ea", new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5361), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 25, 20, 45, 22, 98, DateTimeKind.Unspecified).AddTicks(5361), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FontColorCode",
                table: "UserTemplateDetails",
                newName: "ColorCode");

            migrationBuilder.RenameColumn(
                name: "FontColorCode",
                table: "MasterTemplateDetails",
                newName: "ColorCode");

            migrationBuilder.AddColumn<int>(
                name: "DesignType",
                table: "UserTemplateDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignType",
                table: "MasterTemplateDetails",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7253), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7254), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7244), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7246), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7296), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7296), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7304), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7304), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6dcb031b-aa8a-41ba-ba46-b4b8fba0deef", new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7090), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7090), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "a2fbc748-9670-4eb2-856a-d0c9f5f09d0d", new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7165), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7165), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
