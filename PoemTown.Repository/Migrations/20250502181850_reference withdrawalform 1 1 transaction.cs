using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class referencewithdrawalform11transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WithdrawalFormId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2121), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2121), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2149), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2149), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2014), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2016), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2007), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2008), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2053), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2053), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2062), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(2062), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "b0459518-2969-4ea1-988d-b5a2bddeac3d", new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(1846), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(1846), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "be89b849-b555-45f1-958d-4732c30633d3", new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(1919), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 3, 1, 18, 48, 876, DateTimeKind.Unspecified).AddTicks(1919), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WithdrawalFormId",
                table: "Transactions",
                column: "WithdrawalFormId",
                unique: true,
                filter: "[WithdrawalFormId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_WithdrawalForms_WithdrawalFormId",
                table: "Transactions",
                column: "WithdrawalFormId",
                principalTable: "WithdrawalForms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_WithdrawalForms_WithdrawalFormId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_WithdrawalFormId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WithdrawalFormId",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6393), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6393), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6421), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6421), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6287), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6289), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6278), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6279), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6329), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6329), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6338), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6338), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "ef94266e-fd71-49e6-8583-74378f15e515", new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6127), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6127), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "084db384-1ea2-4c17-9b77-6ec4bf18113a", new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6202), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 16, 30, 4, 332, DateTimeKind.Unspecified).AddTicks(6202), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
