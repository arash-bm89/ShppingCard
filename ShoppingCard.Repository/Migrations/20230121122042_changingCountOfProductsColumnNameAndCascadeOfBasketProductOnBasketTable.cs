using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCard.Repository.Migrations
{
    /// <inheritdoc />
    public partial class changingCountOfProductsColumnNameAndCascadeOfBasketProductOnBasketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Baskets_BasketId",
                table: "BasketProducts");


            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Baskets_BasketId",
                table: "BasketProducts",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Baskets_BasketId",
                table: "BasketProducts");


            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Baskets_BasketId",
                table: "BasketProducts",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
