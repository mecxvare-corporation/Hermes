using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFriendAndFollowerEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowerUser_Follower_FollowersId",
                table: "FollowerUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowerUser_Users_UsersId",
                table: "FollowerUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendUser_Friend_FriendsId",
                table: "FriendUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendUser_Users_UsersId",
                table: "FriendUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "FriendUser",
                newName: "FriendsId1");

            migrationBuilder.RenameIndex(
                name: "IX_FriendUser_UsersId",
                table: "FriendUser",
                newName: "IX_FriendUser_FriendsId1");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "FollowerUser",
                newName: "FollowersId1");

            migrationBuilder.RenameIndex(
                name: "IX_FollowerUser_UsersId",
                table: "FollowerUser",
                newName: "IX_FollowerUser_FollowersId1");

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceUserId",
                table: "Friend",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceUserId",
                table: "Follower",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_FollowerUser_Follower_FollowersId1",
                table: "FollowerUser",
                column: "FollowersId1",
                principalTable: "Follower",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowerUser_Users_FollowersId",
                table: "FollowerUser",
                column: "FollowersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendUser_Friend_FriendsId1",
                table: "FriendUser",
                column: "FriendsId1",
                principalTable: "Friend",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendUser_Users_FriendsId",
                table: "FriendUser",
                column: "FriendsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowerUser_Follower_FollowersId1",
                table: "FollowerUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowerUser_Users_FollowersId",
                table: "FollowerUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendUser_Friend_FriendsId1",
                table: "FriendUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendUser_Users_FriendsId",
                table: "FriendUser");

            migrationBuilder.DropColumn(
                name: "ReferenceUserId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "ReferenceUserId",
                table: "Follower");

            migrationBuilder.RenameColumn(
                name: "FriendsId1",
                table: "FriendUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendUser_FriendsId1",
                table: "FriendUser",
                newName: "IX_FriendUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "FollowersId1",
                table: "FollowerUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowerUser_FollowersId1",
                table: "FollowerUser",
                newName: "IX_FollowerUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowerUser_Follower_FollowersId",
                table: "FollowerUser",
                column: "FollowersId",
                principalTable: "Follower",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowerUser_Users_UsersId",
                table: "FollowerUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendUser_Friend_FriendsId",
                table: "FriendUser",
                column: "FriendsId",
                principalTable: "Friend",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendUser_Users_UsersId",
                table: "FriendUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
