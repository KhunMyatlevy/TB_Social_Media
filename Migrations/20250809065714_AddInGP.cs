using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloWorldApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInGP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
