using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class adduserintorecordfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RecordFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8608), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8608), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8633), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8633), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8492), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8493), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8483), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8485), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8539), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8539), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8547), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8547), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "8f859c97-e9e4-4a5a-8225-4fe50e25f5a2", new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8260), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8260), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "247f803e-392f-407d-98d4-b1c65c646e88", new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8398), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 19, 5, 19, 416, DateTimeKind.Unspecified).AddTicks(8398), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_RecordFiles_UserId",
                table: "RecordFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordFiles_Users_UserId",
                table: "RecordFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordFiles_Users_UserId",
                table: "RecordFiles");

            migrationBuilder.DropIndex(
                name: "IX_RecordFiles_UserId",
                table: "RecordFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RecordFiles");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6506), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6506), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6548), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6548), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6308), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6309), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6294), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6296), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6392), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6392), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6405), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6405), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "5cf630ff-c7f8-4006-9a4c-19619c34ef49", new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(5960), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(5960), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "9216d878-c4e4-487a-bada-a915f08d511c", new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6140), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 21, 18, 59, 37, 791, DateTimeKind.Unspecified).AddTicks(6140), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
