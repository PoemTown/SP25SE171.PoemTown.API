using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addpoemtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Poems");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PoemHistories");

            migrationBuilder.AddColumn<Guid>(
                name: "PoemTypeId",
                table: "Poems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "PoemHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PoemTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoemTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3296), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3296), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3317), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3317), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3195), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3197), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3187), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3188), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3233), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3233), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3239), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3239), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "5de53b5b-a13e-4deb-abe4-4a3ee6b8b184", new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3053), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3053), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "2450a29a-a2ad-4f0d-92ef-200de81a620b", new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3114), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 17, 21, 18, 2, 260, DateTimeKind.Unspecified).AddTicks(3114), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Poems_PoemTypeId",
                table: "Poems",
                column: "PoemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PoemHistories_TypeId",
                table: "PoemHistories",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoemHistories_PoemTypes_TypeId",
                table: "PoemHistories",
                column: "TypeId",
                principalTable: "PoemTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Poems_PoemTypes_PoemTypeId",
                table: "Poems",
                column: "PoemTypeId",
                principalTable: "PoemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoemHistories_PoemTypes_TypeId",
                table: "PoemHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Poems_PoemTypes_PoemTypeId",
                table: "Poems");

            migrationBuilder.DropTable(
                name: "PoemTypes");

            migrationBuilder.DropIndex(
                name: "IX_Poems_PoemTypeId",
                table: "Poems");

            migrationBuilder.DropIndex(
                name: "IX_PoemHistories_TypeId",
                table: "PoemHistories");

            migrationBuilder.DropColumn(
                name: "PoemTypeId",
                table: "Poems");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "PoemHistories");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Poems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PoemHistories",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1933), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1933), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1956), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1956), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1856), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1857), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1848), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1849), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1885), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1885), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1889), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1889), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "036021e2-8740-4157-aa9c-97a4881ddd32", new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1727), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1727), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "2d817e90-623f-4112-b1ee-ae45120112a8", new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1796), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 15, 0, 58, 49, 72, DateTimeKind.Unspecified).AddTicks(1796), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
