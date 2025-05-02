using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class extendbaseentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WithdrawalComplaintImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "WithdrawalComplaintImages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "WithdrawalComplaintImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "WithdrawalComplaintImages",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "WithdrawalComplaintImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdatedTime",
                table: "WithdrawalComplaintImages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7999), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 31, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7999), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(8022), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 31, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(8022), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7864), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7865), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7855), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7856), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7914), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7914), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7928), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7928), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "20f3e187-1cc5-479b-87be-3d64804f3c3d", new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7687), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7687), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "eda69522-8cc8-4430-937a-8b81300d4d63", new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7760), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7760), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WithdrawalComplaintImages");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "WithdrawalComplaintImages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WithdrawalComplaintImages");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "WithdrawalComplaintImages");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "WithdrawalComplaintImages");

            migrationBuilder.DropColumn(
                name: "LastUpdatedTime",
                table: "WithdrawalComplaintImages");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9765), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9765), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9788), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9788), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9656), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9657), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9644), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9645), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9696), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9696), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9702), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9702), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "60654ffa-9602-431d-8781-01b8947e24a4", new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9515), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9515), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6c4861f7-cf11-4361-8ffa-269f4fff2a40", new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9572), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 0, 55, 39, 109, DateTimeKind.Unspecified).AddTicks(9572), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
