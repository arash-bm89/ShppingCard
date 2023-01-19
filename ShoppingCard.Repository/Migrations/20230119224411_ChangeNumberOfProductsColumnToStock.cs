using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCard.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNumberOfProductsColumnToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasketCacheKey",
                table: "Baskets");

            migrationBuilder.RenameColumn(
                name: "NumberOfAvailable",
                table: "Products",
                newName: "Stock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BasketCacheKey",
                table: "Baskets",
                type: "uuid",
                nullable: true);
        }
    }
}
