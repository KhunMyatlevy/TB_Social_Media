using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloWorldApi.Migrations
{
    /// <inheritdoc />
    public partial class tab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Groups",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                newName: "IX_Groups_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Groups",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                newName: "IX_Groups_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
