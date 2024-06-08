using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBarn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseHistoryEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_PurchaseHistories_PurchaseHistoryId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PurchaseHistoryId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PurchaseHistoryId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "CartId",
                table: "PurchaseHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "PurchaseHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PurchaseHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistories_CartId",
                table: "PurchaseHistories",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistories_Carts_CartId",
                table: "PurchaseHistories",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistories_Carts_CartId",
                table: "PurchaseHistories");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseHistories_CartId",
                table: "PurchaseHistories");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "PurchaseHistories");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PurchaseHistories");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PurchaseHistories");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseHistoryId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PurchaseHistoryId",
                table: "Books",
                column: "PurchaseHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_PurchaseHistories_PurchaseHistoryId",
                table: "Books",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id");
        }
    }
}
