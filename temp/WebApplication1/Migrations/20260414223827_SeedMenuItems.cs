using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class SeedMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================
            // PARENT ITEMS
            // =========================
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Title", "Controller", "Action", "PermissionId", "ParentId" },
                values: new object[,]
                {
                    { 1, "Products", null, null, null, null },
                    { 2, "Cart", null, null, null, null }
                });

            // =========================
            // CHILD - PRODUCTS
            // =========================
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Title", "Controller", "Action", "PermissionId", "ParentId" },
                values: new object[,]
                {
                    // View Products
                    { 3, "All Products", "Product", "Index", 1, 1 },

                    // Create Product
                    { 4, "Create Product", "Product", "Create", 3, 1 }
                });

            // =========================
            // CHILD - CART
            // =========================
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Title", "Controller", "Action", "PermissionId", "ParentId" },
                values: new object[,]
                {
                    // View Cart
                    { 5, "View Cart", "Cart", "MyCart", 7, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM MenuItems");
        }
    }
}