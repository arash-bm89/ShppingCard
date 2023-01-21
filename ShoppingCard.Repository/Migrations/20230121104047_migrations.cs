using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCard.Repository.Migrations
{
    /// <inheritdoc />
    public partial class migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountOfProduct",
                table: "BasketProducts",
                newName: "Count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "BasketProducts",
                newName: "CountOfProduct");
        }
    }
}
