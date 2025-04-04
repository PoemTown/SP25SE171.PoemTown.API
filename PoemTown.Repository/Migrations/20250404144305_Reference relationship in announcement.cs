using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoemTown.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Referencerelationshipinannouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AchievementId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CollectionId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LikeId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PoemId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PoemLeaderboardId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserLeaderboardId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(6023), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(6023), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(6047), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 30, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(6047), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5907), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5909), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5899), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5900), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5949), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5949), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5956), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5956), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "b0597dcc-1342-462c-8eb1-226bd547a209", new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5740), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5740), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "d045c49e-3c9e-411e-8f4f-a2348fe29098", new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5809), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 4, 21, 43, 4, 508, DateTimeKind.Unspecified).AddTicks(5809), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AchievementId",
                table: "Announcements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CollectionId",
                table: "Announcements",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CommentId",
                table: "Announcements",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_LikeId",
                table: "Announcements",
                column: "LikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_PoemId",
                table: "Announcements",
                column: "PoemId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_PoemLeaderboardId",
                table: "Announcements",
                column: "PoemLeaderboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ReportId",
                table: "Announcements",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TransactionId",
                table: "Announcements",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_UserLeaderboardId",
                table: "Announcements",
                column: "UserLeaderboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Achievements_AchievementId",
                table: "Announcements",
                column: "AchievementId",
                principalTable: "Achievements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Collections_CollectionId",
                table: "Announcements",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Comments_CommentId",
                table: "Announcements",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Likes_LikeId",
                table: "Announcements",
                column: "LikeId",
                principalTable: "Likes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_PoemLeaderBoards_PoemLeaderboardId",
                table: "Announcements",
                column: "PoemLeaderboardId",
                principalTable: "PoemLeaderBoards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Poems_PoemId",
                table: "Announcements",
                column: "PoemId",
                principalTable: "Poems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Reports_ReportId",
                table: "Announcements",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Transactions_TransactionId",
                table: "Announcements",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_UserLeaderBoards_UserLeaderboardId",
                table: "Announcements",
                column: "UserLeaderboardId",
                principalTable: "UserLeaderBoards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Achievements_AchievementId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Collections_CollectionId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Comments_CommentId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Likes_LikeId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_PoemLeaderBoards_PoemLeaderboardId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Poems_PoemId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Reports_ReportId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Transactions_TransactionId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_UserLeaderBoards_UserLeaderboardId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_AchievementId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_CollectionId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_CommentId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_LikeId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_PoemId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_PoemLeaderboardId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_ReportId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_TransactionId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_UserLeaderboardId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "AchievementId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "PoemId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "PoemLeaderboardId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "UserLeaderboardId",
                table: "Announcements");

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5262e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3569), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3569), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "LeaderBoards",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f5293e"),
                columns: new[] { "CreatedTime", "EndDate", "LastUpdatedTime", "StartDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3587), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 23, 59, 59, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3587), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("89fca251-f021-425b-de62-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3439), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3440), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"),
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3429), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3431), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b74c0a77-a451-4f16-de61-08dcdfcdb851"), new Guid("094de1df-60b1-4a58-878c-dc6909f7350b") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3498), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3498), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("89fca251-f021-425b-de62-08dcdfcdb851"), new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359") },
                columns: new[] { "CreatedTime", "LastUpdatedTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3507), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3507), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "0e35818f-2fa8-4c87-80f8-54d8e87468fe", new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3268), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3268), new TimeSpan(0, 7, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime" },
                values: new object[] { "06392c33-b18f-4906-8fc5-93ddf590c371", new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3335), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 3, 31, 21, 59, 56, 148, DateTimeKind.Unspecified).AddTicks(3335), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
