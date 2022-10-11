using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Seles.Migrations
{
    public partial class add_order_serial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderSerial",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderSerial",
                table: "Orders");
        }
    }
}
