using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_pets_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "full_name_second_name",
                table: "volunteers",
                newName: "full_name_patronymic");

            migrationBuilder.RenameColumn(
                name: "address_street",
                table: "pets",
                newName: "street");

            migrationBuilder.RenameColumn(
                name: "address_postal_code",
                table: "pets",
                newName: "postal_code");

            migrationBuilder.RenameColumn(
                name: "address_house_number",
                table: "pets",
                newName: "house_number");

            migrationBuilder.RenameColumn(
                name: "address_flat_number",
                table: "pets",
                newName: "flat_number");

            migrationBuilder.RenameColumn(
                name: "address_country",
                table: "pets",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "address_city",
                table: "pets",
                newName: "city");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "full_name_patronymic",
                table: "volunteers",
                newName: "full_name_second_name");

            migrationBuilder.RenameColumn(
                name: "street",
                table: "pets",
                newName: "address_street");

            migrationBuilder.RenameColumn(
                name: "postal_code",
                table: "pets",
                newName: "address_postal_code");

            migrationBuilder.RenameColumn(
                name: "house_number",
                table: "pets",
                newName: "address_house_number");

            migrationBuilder.RenameColumn(
                name: "flat_number",
                table: "pets",
                newName: "address_flat_number");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "pets",
                newName: "address_country");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "pets",
                newName: "address_city");
        }
    }
}
