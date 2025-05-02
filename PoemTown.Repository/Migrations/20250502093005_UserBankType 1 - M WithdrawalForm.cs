using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UserBankType1MWithdrawalForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBankTypes_BankTypes_BankTypeId",
                table: "UserBankTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBankTypes_Users_UserId",
                table: "UserBankTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "UserBankTypeId",
                table: "WithdrawalForms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserBankTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BankTypeId",
                table: "UserBankTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalForms_UserBankTypeId",
                table: "WithdrawalForms",
                column: "UserBankTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankTypes_BankTypes_BankTypeId",
                table: "UserBankTypes",
                column: "BankTypeId",
                principalTable: "BankTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankTypes_Users_UserId",
                table: "UserBankTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawalForms_UserBankTypes_UserBankTypeId",
                table: "WithdrawalForms",
                column: "UserBankTypeId",
                principalTable: "UserBankTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBankTypes_BankTypes_BankTypeId",
                table: "UserBankTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBankTypes_Users_UserId",
                table: "UserBankTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawalForms_UserBankTypes_UserBankTypeId",
                table: "WithdrawalForms");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawalForms_UserBankTypeId",
                table: "WithdrawalForms");

            migrationBuilder.DropColumn(
                name: "UserBankTypeId",
                table: "WithdrawalForms");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserBankTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BankTypeId",
                table: "UserBankTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9148), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9148), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9171), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9171), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9037), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9038), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9029), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9030), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9081), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9081), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9089), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(9089), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6dfcf385-ccf6-4f7c-8fcb-9e5fedee67a9", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8879), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8879), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "f24d7c91-798c-4c3e-9f5d-aeb28f282237", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8952), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 57, 3, 921, DateTimeKind.Unspecified).AddTicks(8952), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankTypes_BankTypes_BankTypeId",
                table: "UserBankTypes",
                column: "BankTypeId",
                principalTable: "BankTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankTypes_Users_UserId",
                table: "UserBankTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
