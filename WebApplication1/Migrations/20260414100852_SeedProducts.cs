using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 50; i++)
            {
                migrationBuilder.InsertData(
                    table: "Products",
                    columns: new[] { "Name", "Price", "Quantity" },
                    values: new object[]
                    {
                $"Product {i}",
                10 + i,
                100 + i
                    });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products");
        }
    }
}
