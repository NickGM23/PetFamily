using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_isoftdelete_for_species_and_breed_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "species",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "breeds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "species");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "breeds");
        }
    }
}
