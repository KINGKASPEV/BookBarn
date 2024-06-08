using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBarn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseHistoryUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Checkouts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_AppUserId",
                table: "Checkouts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_AspNetUsers_AppUserId",
                table: "Checkouts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_AspNetUsers_AppUserId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_AppUserId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Checkouts");
        }
    }
}
