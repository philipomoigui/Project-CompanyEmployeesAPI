using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployees.Migrations
{
    public partial class ExtendedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19a3f154-58ef-4918-8cc7-745a5f03cb49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b640438-7204-41a0-8e8b-76112c0df894");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9eaaa21f-d8d8-44fb-9ed8-4093500f1ea0", "5fd69cfc-c9fa-434e-ab11-d4585070235e", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b76fbb0d-9f04-4983-8f6e-ccf460eba0a3", "14b0a5db-a48c-4b8e-b946-5f50bc38ead5", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaaa21f-d8d8-44fb-9ed8-4093500f1ea0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b76fbb0d-9f04-4983-8f6e-ccf460eba0a3");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2b640438-7204-41a0-8e8b-76112c0df894", "e7c099ec-ecc6-4b54-b048-e9ffafd6f2a5", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "19a3f154-58ef-4918-8cc7-745a5f03cb49", "cb6bf9d1-2a4a-4049-b882-840c2af173a5", "Administrator", "ADMINISTRATOR" });
        }
    }
}
