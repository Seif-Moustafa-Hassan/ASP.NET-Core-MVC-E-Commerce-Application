using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Admin user -> Admin role
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[]
                {
                    "c496ad60-2a69-4345-af71-d00d368b0411", // admin@gmail.com
                    "505c99d2-a131-43c1-b4f9-ecd8106dd9d5"  // Admin role
                });

            // Normal user -> User role
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[]
                {
                    "1fe9759b-2a32-4a2f-b24a-c254707d3767", // user@gmail.com
                    "58a2a767-b211-4245-8644-c215b845550c"  // User role
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[]
                {
                    "c496ad60-2a69-4345-af71-d00d368b0411",
                    "505c99d2-a131-43c1-b4f9-ecd8106dd9d5"
                });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[]
                {
                    "1fe9759b-2a32-4a2f-b24a-c254707d3767",
                    "58a2a767-b211-4245-8644-c215b845550c"
                });
        }
    }
}
