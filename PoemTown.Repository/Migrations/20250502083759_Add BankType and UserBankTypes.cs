using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddBankTypeandUserBankTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankType",
                table: "WithdrawalForms");

            migrationBuilder.AddColumn<Guid>(
                name: "BankTypeId",
                table: "WithdrawalForms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBankTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBankTypes_BankTypes_BankTypeId",
                        column: x => x.BankTypeId,
                        principalTable: "BankTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBankTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9126), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9126), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9155), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9155), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8999), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9000), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8989), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8990), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9056), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9056), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9064), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(9064), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "8d9a2f97-309e-4a6a-bc25-3263b510d745", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8821), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "4e026b7b-7186-4c04-92e2-990ee3c374da", new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8903), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 15, 37, 57, 601, DateTimeKind.Unspecified).AddTicks(8903), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalForms_BankTypeId",
                table: "WithdrawalForms",
                column: "BankTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankTypes_BankTypeId",
                table: "UserBankTypes",
                column: "BankTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankTypes_UserId",
                table: "UserBankTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawalForms_BankTypes_BankTypeId",
                table: "WithdrawalForms",
                column: "BankTypeId",
                principalTable: "BankTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawalForms_BankTypes_BankTypeId",
                table: "WithdrawalForms");

            migrationBuilder.DropTable(
                name: "UserBankTypes");

            migrationBuilder.DropTable(
                name: "BankTypes");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawalForms_BankTypeId",
                table: "WithdrawalForms");

            migrationBuilder.DropColumn(
                name: "BankTypeId",
                table: "WithdrawalForms");

            migrationBuilder.AddColumn<int>(
                name: "BankType",
                table: "WithdrawalForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7999), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(7999), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(8022), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 2, 2, 28, 10, 878, DateTimeKind.Unspecified).AddTicks(8022), new TimeSpan(0, 7, 0, 0, 0)) });

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
    }
}
