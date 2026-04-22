using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api_clase.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "juan@email.com", true, "Juan Pérez", "password123", "ADMIN" },
                    { 2, new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria@email.com", true, "María García", "pass1234", "CLIENT" },
                    { 3, new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "carlos@email.com", true, "Carlos López", "secure456", "CLIENT" }
                });

            migrationBuilder.InsertData(
                table: "Phones",
                columns: new[] { "Id", "Brand", "IsActive", "Model", "Price", "ReleaseDate", "Stock" },
                values: new object[,]
                {
                    { 1, "Apple", true, "iPhone 15", 999.99m, new DateTime(2024, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 50 },
                    { 2, "Samsung", true, "Galaxy S24", 899.99m, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 40 },
                    { 3, "Google", true, "Pixel 8", 799.99m, new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 30 },
                    { 4, "OnePlus", true, "12", 699.99m, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 25 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Phones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Phones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Phones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Phones",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
