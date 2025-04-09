using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class nowdepositmoneyintowalletwilltransferdataintotransactioninsteadoforder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentGateways_PaymentGatewayId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentGatewayId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayId",
                table: "Orders");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CancelledDate",
                table: "Transactions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaidDate",
                table: "Transactions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentGatewayId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionToken",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9910), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9910), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9951), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9951), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9725), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9727), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9711), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9714), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9803), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9803), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9818), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9818), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "35d47cd9-0207-43cf-a35b-ccae5a334d76", new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9342), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9342), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "9060cd25-e94f-4018-9291-6c1b61061b27", new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9571), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 20, 39, 51, 828, DateTimeKind.Unspecified).AddTicks(9571), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentGatewayId",
                table: "Transactions",
                column: "PaymentGatewayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentGateways_PaymentGatewayId",
                table: "Transactions",
                column: "PaymentGatewayId",
                principalTable: "PaymentGateways",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentGateways_PaymentGatewayId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentGatewayId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionToken",
                table: "Transactions");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentGatewayId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5543), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5543), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5569), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5569), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5389), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5390), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5379), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5380), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5454), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5454), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5461), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5461), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "77b608b9-4cc3-419b-8c50-5701b1e5c774", new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5067), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5067), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "75076e9e-9720-4205-a294-12d3f18ad103", new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5142), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 9, 13, 15, 34, 774, DateTimeKind.Unspecified).AddTicks(5142), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentGatewayId",
                table: "Orders",
                column: "PaymentGatewayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentGateways_PaymentGatewayId",
                table: "Orders",
                column: "PaymentGatewayId",
                principalTable: "PaymentGateways",
                principalColumn: "Id");
        }
    }
}
