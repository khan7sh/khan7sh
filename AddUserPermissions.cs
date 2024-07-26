using Microsoft.EntityFrameworkCore.Migrations;

namespace BasicVendorInventoryPlatform.Migrations
{
    public partial class AddUserPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserPermissions",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPermissions",
                table: "Users");
        }
    }
}