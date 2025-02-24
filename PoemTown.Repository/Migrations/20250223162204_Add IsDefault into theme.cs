using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefaultintotheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Themes",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9292), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9293), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9285), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9286), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9329), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9329), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9336), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9336), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6f59db7c-6cae-41d4-941b-126c1d226119", new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9159), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9159), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "804ed230-6f87-416f-91ae-6dde3e083afd", new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9221), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9221), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Themes");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3652), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3653), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3643), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3644), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3686), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3686), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3690), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3690), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "f520bd11-9ed6-47ea-a67e-9ff91090a3cd", new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3543), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3543), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "32ffb3c7-bcb2-4392-b10c-2be82304706c", new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3596), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 20, 1, 2, 28, 343, DateTimeKind.Unspecified).AddTicks(3596), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
