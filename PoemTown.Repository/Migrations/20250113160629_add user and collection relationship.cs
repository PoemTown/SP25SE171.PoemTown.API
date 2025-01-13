using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class adduserandcollectionrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BalacneAfter",
                table: "Transactions",
                newName: "BalanceAfter");

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
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4582), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4583), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4576), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4577), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4612), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4612), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4616), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4616), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "679336db-bf8c-40b4-8405-f0b44cfc1655", new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4454), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4454), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "e04b5b05-a853-4e8c-a369-e5cf3d163310", new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4512), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 23, 6, 28, 652, DateTimeKind.Unspecified).AddTicks(4512), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Poems");

            migrationBuilder.RenameColumn(
                name: "BalanceAfter",
                table: "Transactions",
                newName: "BalacneAfter");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2812), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2813), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2806), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2807), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2919), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2919), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2929), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2929), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "49cef343-ed18-4832-801f-85ef0e158761", new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2706), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2706), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "825cd233-6686-4410-a3d6-b75864b73a4e", new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2752), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 13, 16, 4, 21, 671, DateTimeKind.Unspecified).AddTicks(2752), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
