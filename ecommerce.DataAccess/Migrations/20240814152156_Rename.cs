using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppiongCarts_AspNetUsers_UserId",
                table: "ShoppiongCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppiongCarts_Products_ProductId",
                table: "ShoppiongCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppiongCarts",
                table: "ShoppiongCarts");

            migrationBuilder.RenameTable(
                name: "ShoppiongCarts",
                newName: "ShoppingCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppiongCarts_UserId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppiongCarts_ProductId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products_ProductId",
                table: "ShoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts");

            migrationBuilder.RenameTable(
                name: "ShoppingCarts",
                newName: "ShoppiongCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppiongCarts",
                newName: "IX_ShoppiongCarts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppiongCarts",
                newName: "IX_ShoppiongCarts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppiongCarts",
                table: "ShoppiongCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppiongCarts_AspNetUsers_UserId",
                table: "ShoppiongCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppiongCarts_Products_ProductId",
                table: "ShoppiongCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
