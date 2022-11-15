using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class AddDefaulRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "83c5b7b6-2050-42d2-a105-d6a18df4c6f3", "7d17be2e-bd1b-4636-a099-02a49f895c28", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "83c5b7b6-2050-42d2-a105-d4a18df4c6f3", "7d17be2e-b21b-4636-a099-02a49f895c28", "Editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ed0544da-4d19-4c49-b771-50d77a4f1c94", "d075b481-335b-4e74-9e3a-902fed52d781", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83c5b7b6-2050-42d2-a105-d6a18df4c6f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed0544da-4d19-4c49-b771-50d77a4f1c94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83c5b7b6-2050-42d2-a105-d4a18df4c6f3");
        }
    }
}
