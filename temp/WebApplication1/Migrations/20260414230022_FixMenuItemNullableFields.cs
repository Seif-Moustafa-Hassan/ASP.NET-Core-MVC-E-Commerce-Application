using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class FixMenuItemNullableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make Controller nullable
            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Make Action nullable
            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // revert back to NOT NULL (only safe if data supports it)
            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}