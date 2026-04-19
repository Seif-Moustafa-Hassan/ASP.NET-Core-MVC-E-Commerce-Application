using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ReseedPermissionsAndRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean first (avoid duplicates)
            migrationBuilder.Sql("DELETE FROM RolePermissions");
            migrationBuilder.Sql("DELETE FROM Permissions");

            // Seed Permissions
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
            { 1, "View Products" },
            { 2, "View Product Details" },
            { 3, "Create Product" },
            { 4, "Update Product" },
            { 5, "Delete Product" },
            { 6, "Add to Cart" },
            { 7, "View Cart" }
                });

            var adminRoleId = "505c99d2-a131-43c1-b4f9-ecd8106dd9d5";
            var userRoleId = "58a2a767-b211-4245-8644-c215b845550c";

            // Admin permissions
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
            { adminRoleId, 1 },
            { adminRoleId, 2 },
            { adminRoleId, 3 },
            { adminRoleId, 4 },
            { adminRoleId, 5 }
                });

            // User permissions
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
            { userRoleId, 1 },
            { userRoleId, 2 },
            { userRoleId, 6 },
            { userRoleId, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
