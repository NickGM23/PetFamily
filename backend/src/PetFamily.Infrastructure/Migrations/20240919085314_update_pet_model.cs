﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_pet_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Requisites",
                table: "pets",
                newName: "requisites");

            migrationBuilder.RenameColumn(
                name: "PetPhotos",
                table: "pets",
                newName: "photos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "requisites",
                table: "pets",
                newName: "Requisites");

            migrationBuilder.RenameColumn(
                name: "photos",
                table: "pets",
                newName: "PetPhotos");
        }
    }
}
