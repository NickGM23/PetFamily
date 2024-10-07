using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_isrequired_for_foriegnkyes_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_species_species_id",
                table: "breeds");

            migrationBuilder.AlterColumn<Guid>(
                name: "species_id",
                table: "breeds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_species_species_id",
                table: "breeds",
                column: "species_id",
                principalTable: "species",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_species_species_id",
                table: "breeds");

            migrationBuilder.AlterColumn<Guid>(
                name: "species_id",
                table: "breeds",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_species_species_id",
                table: "breeds",
                column: "species_id",
                principalTable: "species",
                principalColumn: "id");
        }
    }
}
