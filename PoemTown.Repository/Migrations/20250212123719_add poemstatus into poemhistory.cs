using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addpoemstatusintopoemhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6866), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6859), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6860), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6921), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6921), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6928), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6928), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "3088a4c6-ea98-4986-9d72-f965e24ce242", new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6668), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6668), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "7f1e4fc5-782f-4681-8375-9a66a57967d1", new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6742), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 12, 19, 37, 18, 795, DateTimeKind.Unspecified).AddTicks(6742), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1026), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1027), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1019), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1021), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1062), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1062), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1067), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(1067), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "88875b4c-e991-4dad-83f6-94d1f979f167", new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(896), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(896), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "ce4957fb-66b4-4a29-b200-a875de85c005", new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(951), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 11, 21, 45, 20, 848, DateTimeKind.Unspecified).AddTicks(951), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
