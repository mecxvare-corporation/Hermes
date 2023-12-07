using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedNavigationPropertuForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestUser_Users_UserId",
                table: "InterestUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "InterestUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_InterestUser_UserId",
                table: "InterestUser",
                newName: "IX_InterestUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestUser_Users_UsersId",
                table: "InterestUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestUser_Users_UsersId",
                table: "InterestUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "InterestUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_InterestUser_UsersId",
                table: "InterestUser",
                newName: "IX_InterestUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestUser_Users_UserId",
                table: "InterestUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
