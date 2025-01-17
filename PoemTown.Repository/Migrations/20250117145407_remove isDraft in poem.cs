using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class removeisDraftinpoem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Poems");

            migrationBuilder.RenameColumn(
                name: "PoemStatus",
                table: "Poems",
                newName: "Status");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Poems",
                newName: "PoemStatus");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Poems",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(232), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(233), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(226), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(227), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(260), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(260), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(265), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(265), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "9c4b9728-c282-433b-b3ba-fc30fada377d", new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(104), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(104), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "70fa96dd-df87-45a1-a0e0-d13d77a4007f", new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(162), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 16, 3, 12, 26, 661, DateTimeKind.Unspecified).AddTicks(162), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
