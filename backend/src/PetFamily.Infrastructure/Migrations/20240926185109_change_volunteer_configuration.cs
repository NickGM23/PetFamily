using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_volunteer_configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Requisites",
                table: "volunteers",
                newName: "requisites");

            migrationBuilder.RenameColumn(
                name: "SocialNetworks",
                table: "volunteers",
                newName: "social_networks");

            migrationBuilder.AddColumn<int>(
                name: "serial_number",
                table: "pets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "pets");

            migrationBuilder.RenameColumn(
                name: "requisites",
                table: "volunteers",
                newName: "Requisites");

            migrationBuilder.RenameColumn(
                name: "social_networks",
                table: "volunteers",
                newName: "SocialNetworks");
        }
    }
}
