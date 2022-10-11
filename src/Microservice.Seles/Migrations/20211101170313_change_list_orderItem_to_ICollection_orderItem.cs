using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Seles.Migrations
{
    public partial class change_list_orderItem_to_ICollection_orderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "Password", "Phone", "Role", "Serial" },
                values: new object[] { 1, "محمد صدرا برومند", "sadra123", "09140286763", "admin", "306334c0-2e87-4d9d-b682-8af027dae1b7" });
        }
    }
}
