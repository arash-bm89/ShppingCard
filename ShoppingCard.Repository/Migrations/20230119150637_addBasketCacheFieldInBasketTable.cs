using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCard.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addBasketCacheFieldInBasketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BasketCacheKey",
                table: "Baskets",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasketCacheKey",
                table: "Baskets");
        }
    }
}
