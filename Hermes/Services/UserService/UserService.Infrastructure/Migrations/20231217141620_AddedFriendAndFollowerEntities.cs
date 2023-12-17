using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedFriendAndFollowerEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Follower",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follower", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowerUser",
                columns: table => new
                {
                    FollowersId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerUser", x => new { x.FollowersId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_FollowerUser_Follower_FollowersId",
                        column: x => x.FollowersId,
                        principalTable: "Follower",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowerUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendUser",
                columns: table => new
                {
                    FriendsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendUser", x => new { x.FriendsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_FriendUser_Friend_FriendsId",
                        column: x => x.FriendsId,
                        principalTable: "Friend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUser_UsersId",
                table: "FollowerUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendUser_UsersId",
                table: "FriendUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowerUser");

            migrationBuilder.DropTable(
                name: "FriendUser");

            migrationBuilder.DropTable(
                name: "Follower");

            migrationBuilder.DropTable(
                name: "Friend");
        }
    }
}
