using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsystemContactsanddailyMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInUse = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemContacts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9591), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9591), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9622), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9622), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9394), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9396), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9378), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9380), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9474), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9474), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9488), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9488), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "0725757f-65a9-4b03-94dc-6e0438e3aa5f", new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9143), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9143), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "c9ada992-afe4-4cf4-90d9-ac1d89f48b1f", new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9232), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 20, 16, 55, 51, 958, DateTimeKind.Unspecified).AddTicks(9232), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMessages");

            migrationBuilder.DropTable(
                name: "SystemContacts");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8679), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8679), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8742), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8742), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8457), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8459), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8439), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8441), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8542), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8542), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8554), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8554), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "4fdd2812-a14c-4d30-baef-d9abad53ac3f", new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8120), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8120), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "98e748ab-2db6-4c36-ae0b-f54e07b12302", new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8288), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 19, 21, 7, 22, 985, DateTimeKind.Unspecified).AddTicks(8288), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
