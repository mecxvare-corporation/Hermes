using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFriendsAndFollowerTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowerId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_UserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_FriendId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_UserId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                table: "Followers");

            migrationBuilder.RenameTable(
                name: "Friends",
                newName: "UserFriend");

            migrationBuilder.RenameTable(
                name: "Followers",
                newName: "UserFollower");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_UserId",
                table: "UserFriend",
                newName: "IX_UserFriend_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_FriendId",
                table: "UserFriend",
                newName: "IX_UserFriend_FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_UserId",
                table: "UserFollower",
                newName: "IX_UserFollower_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowerId",
                table: "UserFollower",
                newName: "IX_UserFollower_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFriend",
                table: "UserFriend",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollower",
                table: "UserFollower",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollower_Users_FollowerId",
                table: "UserFollower",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollower_Users_UserId",
                table: "UserFollower",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriend_Users_FriendId",
                table: "UserFriend",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriend_Users_UserId",
                table: "UserFriend",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollower_Users_FollowerId",
                table: "UserFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollower_Users_UserId",
                table: "UserFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_Users_FriendId",
                table: "UserFriend");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_Users_UserId",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFriend",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollower",
                table: "UserFollower");

            migrationBuilder.RenameTable(
                name: "UserFriend",
                newName: "Friends");

            migrationBuilder.RenameTable(
                name: "UserFollower",
                newName: "Followers");

            migrationBuilder.RenameIndex(
                name: "IX_UserFriend_UserId",
                table: "Friends",
                newName: "IX_Friends_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFriend_FriendId",
                table: "Friends",
                newName: "IX_Friends_FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollower_UserId",
                table: "Followers",
                newName: "IX_Followers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollower_FollowerId",
                table: "Followers",
                newName: "IX_Followers_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                table: "Followers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowerId",
                table: "Followers",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_UserId",
                table: "Followers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_FriendId",
                table: "Friends",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
