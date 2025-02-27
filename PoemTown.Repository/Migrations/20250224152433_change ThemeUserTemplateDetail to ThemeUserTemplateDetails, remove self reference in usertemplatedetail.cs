using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class changeThemeUserTemplateDetailtoThemeUserTemplateDetailsremoveselfreferenceinusertemplatedetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThemeUserTemplateDetail_Themes_ThemeId",
                table: "ThemeUserTemplateDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ThemeUserTemplateDetail_UserTemplateDetails_UserTemplateDetailId",
                table: "ThemeUserTemplateDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTemplateDetails_UserTemplateDetails_ParentTemplateDetailId",
                table: "UserTemplateDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserTemplateDetails_ParentTemplateDetailId",
                table: "UserTemplateDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThemeUserTemplateDetail",
                table: "ThemeUserTemplateDetail");

            migrationBuilder.DropColumn(
                name: "ParentTemplateDetailId",
                table: "UserTemplateDetails");

            migrationBuilder.RenameTable(
                name: "ThemeUserTemplateDetail",
                newName: "ThemeUserTemplateDetails");

            migrationBuilder.RenameIndex(
                name: "IX_ThemeUserTemplateDetail_UserTemplateDetailId",
                table: "ThemeUserTemplateDetails",
                newName: "IX_ThemeUserTemplateDetails_UserTemplateDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ThemeUserTemplateDetail_ThemeId",
                table: "ThemeUserTemplateDetails",
                newName: "IX_ThemeUserTemplateDetails_ThemeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThemeUserTemplateDetails",
                table: "ThemeUserTemplateDetails",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7253), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7254), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7244), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7246), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7296), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7296), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7304), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7304), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6dcb031b-aa8a-41ba-ba46-b4b8fba0deef", new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7090), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7090), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "a2fbc748-9670-4eb2-856a-d0c9f5f09d0d", new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7165), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 24, 22, 24, 32, 629, DateTimeKind.Unspecified).AddTicks(7165), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeUserTemplateDetails_Themes_ThemeId",
                table: "ThemeUserTemplateDetails",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeUserTemplateDetails_UserTemplateDetails_UserTemplateDetailId",
                table: "ThemeUserTemplateDetails",
                column: "UserTemplateDetailId",
                principalTable: "UserTemplateDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThemeUserTemplateDetails_Themes_ThemeId",
                table: "ThemeUserTemplateDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ThemeUserTemplateDetails_UserTemplateDetails_UserTemplateDetailId",
                table: "ThemeUserTemplateDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThemeUserTemplateDetails",
                table: "ThemeUserTemplateDetails");

            migrationBuilder.RenameTable(
                name: "ThemeUserTemplateDetails",
                newName: "ThemeUserTemplateDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ThemeUserTemplateDetails_UserTemplateDetailId",
                table: "ThemeUserTemplateDetail",
                newName: "IX_ThemeUserTemplateDetail_UserTemplateDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ThemeUserTemplateDetails_ThemeId",
                table: "ThemeUserTemplateDetail",
                newName: "IX_ThemeUserTemplateDetail_ThemeId");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTemplateDetailId",
                table: "UserTemplateDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThemeUserTemplateDetail",
                table: "ThemeUserTemplateDetail",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9292), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9293), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9285), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9286), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9329), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9329), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9336), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9336), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "6f59db7c-6cae-41d4-941b-126c1d226119", new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9159), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9159), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "804ed230-6f87-416f-91ae-6dde3e083afd", new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9221), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 2, 23, 23, 22, 3, 612, DateTimeKind.Unspecified).AddTicks(9221), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_UserTemplateDetails_ParentTemplateDetailId",
                table: "UserTemplateDetails",
                column: "ParentTemplateDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeUserTemplateDetail_Themes_ThemeId",
                table: "ThemeUserTemplateDetail",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeUserTemplateDetail_UserTemplateDetails_UserTemplateDetailId",
                table: "ThemeUserTemplateDetail",
                column: "UserTemplateDetailId",
                principalTable: "UserTemplateDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTemplateDetails_UserTemplateDetails_ParentTemplateDetailId",
                table: "UserTemplateDetails",
                column: "ParentTemplateDetailId",
                principalTable: "UserTemplateDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
