using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class WithdrawalFormtosendawithdrawform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WithdrawalFormId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WithdrawalForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolveDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolveEvidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawalForms_UserEWallets_UserEWalletId",
                        column: x => x.UserEWalletId,
                        principalTable: "UserEWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9815), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9815), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9833), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9833), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9698), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9700), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9691), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9692), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9750), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9750), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9756), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9756), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "acac94f1-e815-4d81-bf77-244ecb3d129a", new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9542), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9542), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "8b090f74-1545-47f5-95ca-b94e20720947", new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9613), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 14, 1, 1, 29, 418, DateTimeKind.Unspecified).AddTicks(9613), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_WithdrawalFormId",
                table: "Announcements",
                column: "WithdrawalFormId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalForms_UserEWalletId",
                table: "WithdrawalForms",
                column: "UserEWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_WithdrawalForms_WithdrawalFormId",
                table: "Announcements",
                column: "WithdrawalFormId",
                principalTable: "WithdrawalForms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_WithdrawalForms_WithdrawalFormId",
                table: "Announcements");

            migrationBuilder.DropTable(
                name: "WithdrawalForms");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_WithdrawalFormId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "WithdrawalFormId",
                table: "Announcements");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5231), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5231), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5261), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5261), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5099), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5100), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5090), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5091), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5156), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5156), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5165), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(5165), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "a11c455a-c5ce-4f78-ba30-ee8b5f5af314", new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(4805), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(4805), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "0402e9fd-f3f5-4005-ba67-e9a4955fd620", new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(4987), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 12, 22, 36, 43, 146, DateTimeKind.Unspecified).AddTicks(4987), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
