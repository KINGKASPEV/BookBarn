using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBarn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistories_Carts_CartId",
                table: "PurchaseHistories");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "PurchaseHistories",
                newName: "CheckoutId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseHistories_CartId",
                table: "PurchaseHistories",
                newName: "IX_PurchaseHistories_CheckoutId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistories_Checkouts_CheckoutId",
                table: "PurchaseHistories",
                column: "CheckoutId",
                principalTable: "Checkouts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseHistories_Checkouts_CheckoutId",
                table: "PurchaseHistories");

            migrationBuilder.RenameColumn(
                name: "CheckoutId",
                table: "PurchaseHistories",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseHistories_CheckoutId",
                table: "PurchaseHistories",
                newName: "IX_PurchaseHistories_CartId");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Books",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseHistories_Carts_CartId",
                table: "PurchaseHistories",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
