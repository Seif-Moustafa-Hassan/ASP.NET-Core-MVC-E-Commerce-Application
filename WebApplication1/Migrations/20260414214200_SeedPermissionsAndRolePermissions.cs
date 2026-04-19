using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class SeedPermissionsAndRolePermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =========================
            // ✅ SEED PERMISSIONS
            // =========================
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

            // =========================
            // ✅ ROLE IDS (from AspNetRoles table)
            // =========================
            var adminRoleId = "505c99d2-a131-43c1-b4f9-ecd8106dd9d5";
            var userRoleId = "58a2a767-b211-4245-8644-c215b845550c";

            // =========================
            // ✅ ADMIN PERMISSIONS
            // =========================
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
                    { adminRoleId, 1 }, // View Products
                    { adminRoleId, 2 }, // View Product Details
                    { adminRoleId, 3 }, // Create Product
                    { adminRoleId, 4 }, // Update Product
                    { adminRoleId, 5 }  // Delete Product
                });

            // =========================
            // ✅ USER PERMISSIONS
            // =========================
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
                    { userRoleId, 1 }, // View Products
                    { userRoleId, 2 }, // View Product Details
                    { userRoleId, 6 }, // Add to Cart
                    { userRoleId, 7 }  // View Cart
                });

            // =========================
            // INDEX (safe to include)
            // =========================
            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove role-permission links first (FK dependency)
            migrationBuilder.Sql("DELETE FROM RolePermissions");

            // Remove permissions
            migrationBuilder.Sql("DELETE FROM Permissions");
        }
    }
}