using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class SeedMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        -- =========================
        -- ROOT: PRODUCTS
        -- =========================
        INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
        VALUES ('Products', NULL, NULL, NULL, NULL);

        DECLARE @ProductsId INT = SCOPE_IDENTITY();

        INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
        VALUES 
        ('All Products', 'Product', 'Index', @ProductsId, 1),
        ('Create Product', 'Product', 'Create', @ProductsId, 3);

        -- =========================
        -- ROOT: CART
        -- =========================
        INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
        VALUES ('Cart', NULL, NULL, NULL, NULL);

        DECLARE @CartId INT = SCOPE_IDENTITY();

        INSERT INTO MenuItems (Title, Controller, Action, ParentId, PermissionId)
        VALUES
        ('View Cart', 'Cart', 'MyCart', @CartId, 7);
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM MenuItems
        WHERE Title IN 
        ('Products', 'All Products', 'Create Product', 'Cart', 'View Cart');
    ");
        }
    }
}
