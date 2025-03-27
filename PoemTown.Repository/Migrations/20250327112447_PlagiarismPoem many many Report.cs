using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class PlagiarismPoemmanymanyReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Poems_PlagiarismFromPoemId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PlagiarismFromPoemId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PlagiarismFromPoemId",
                table: "Reports");

            migrationBuilder.CreateTable(
                name: "PlagiarismPoemReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlagiarismFromPoemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlagiarismPoemReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlagiarismPoemReports_Poems_PlagiarismFromPoemId",
                        column: x => x.PlagiarismFromPoemId,
                        principalTable: "Poems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlagiarismPoemReports_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8413), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8413), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8451), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8451), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8186), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8189), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8172), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8175), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8291), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8291), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8307), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(8307), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "fdcac2d4-ccc3-4a4f-bf7f-3dd9202ea2ec", new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(7874), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(7874), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "fc27514e-2eda-46c5-9ab8-b753488324cf", new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(7993), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 27, 18, 24, 45, 218, DateTimeKind.Unspecified).AddTicks(7993), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_PlagiarismPoemReports_PlagiarismFromPoemId",
                table: "PlagiarismPoemReports",
                column: "PlagiarismFromPoemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlagiarismPoemReports_ReportId",
                table: "PlagiarismPoemReports",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlagiarismPoemReports");

            migrationBuilder.AddColumn<Guid>(
                name: "PlagiarismFromPoemId",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8190), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8190), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8213), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8213), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8087), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8088), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8079), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8081), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8131), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8131), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8137), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(8137), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "bc9b0805-bfa8-4b51-b0e0-c4a8b4138048", new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7936), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7936), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "aab9e630-c66f-4408-99d5-97e7088221fb", new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7998), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 26, 15, 17, 24, 788, DateTimeKind.Unspecified).AddTicks(7998), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PlagiarismFromPoemId",
                table: "Reports",
                column: "PlagiarismFromPoemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Poems_PlagiarismFromPoemId",
                table: "Reports",
                column: "PlagiarismFromPoemId",
                principalTable: "Poems",
                principalColumn: "Id");
        }
    }
}
