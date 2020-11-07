using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployees.Migrations
{
    public partial class CorrectedAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3ebbd0a-c235-4310-b12a-b5ff233113b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8da8da6-12a6-4c8f-86af-c9866d4c8a83");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2b640438-7204-41a0-8e8b-76112c0df894", "e7c099ec-ecc6-4b54-b048-e9ffafd6f2a5", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "19a3f154-58ef-4918-8cc7-745a5f03cb49", "cb6bf9d1-2a4a-4049-b882-840c2af173a5", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19a3f154-58ef-4918-8cc7-745a5f03cb49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b640438-7204-41a0-8e8b-76112c0df894");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3ebbd0a-c235-4310-b12a-b5ff233113b7", "faf8b531-bfd9-4977-8355-e0fbd5b462fd", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b8da8da6-12a6-4c8f-86af-c9866d4c8a83", "ae47092c-d6ee-4182-b59e-909d79366ba2", "Administrator", "Administrator" });
        }
    }
}
