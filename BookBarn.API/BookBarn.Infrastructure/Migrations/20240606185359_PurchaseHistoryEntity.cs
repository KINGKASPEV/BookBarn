using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBarn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchaseHistoryId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseHistories_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_PurchaseHistoryId",
                table: "Books",
                column: "PurchaseHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseHistories_AppUserId",
                table: "PurchaseHistories",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_PurchaseHistories_PurchaseHistoryId",
                table: "Books",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_PurchaseHistories_PurchaseHistoryId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "PurchaseHistories");

            migrationBuilder.DropIndex(
                name: "IX_Books_PurchaseHistoryId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PurchaseHistoryId",
                table: "Books");
        }
    }
}
