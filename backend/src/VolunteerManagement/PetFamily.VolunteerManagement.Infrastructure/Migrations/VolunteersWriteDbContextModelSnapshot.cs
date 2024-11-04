﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.VolunteerManagement.Infrastructure;

#nullable disable

namespace PetFamily.VolunteerManagement.Infrastructure.Migrations
{
    [DbContext(typeof(VolunteersWriteDbContext))]
    partial class VolunteersWriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("volunteers")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.VolunteerManagement.Domain.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("BirthDay")
                        .HasMaxLength(9)
                        .HasColumnType("date")
                        .HasColumnName("birth_day");

                    b.Property<DateTime>("DateCteate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_cteate");

                    b.Property<double>("Height")
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<string>("HelpStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("help_status");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "PetFamily.VolunteerManagement.Domain.Entities.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("country");

                            b1.Property<string>("FlatNumber")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("flat_number");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("house_number");

                            b1.Property<int>("PostalCode")
                                .HasColumnType("integer")
                                .HasColumnName("postal_code");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Breed", "PetFamily.VolunteerManagement.Domain.Entities.Pet.Breed#PetBreed", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Color", "PetFamily.VolunteerManagement.Domain.Entities.Pet.Color#LowTextLength", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("color");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.VolunteerManagement.Domain.Entities.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HealthInfo", "PetFamily.VolunteerManagement.Domain.Entities.Pet.HealthInfo#HighTextLength", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("health_info");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetFamily.VolunteerManagement.Domain.Entities.Pet.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.VolunteerManagement.Domain.Entities.Pet.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SerialNumber", "PetFamily.VolunteerManagement.Domain.Entities.Pet.SerialNumber#SerialNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("serial_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", "volunteers");
                });

            modelBuilder.Entity("PetFamily.VolunteerManagement.Domain.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.VolunteerManagement.Domain.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "PetFamily.VolunteerManagement.Domain.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "PetFamily.VolunteerManagement.Domain.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("last_name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("patronymic");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.VolunteerManagement.Domain.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("YearsExperience", "PetFamily.VolunteerManagement.Domain.Volunteer.YearsExperience#YearsExperience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasMaxLength(2000)
                                .HasColumnType("integer")
                                .HasColumnName("years_experience");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", "volunteers");
                });

            modelBuilder.Entity("PetFamily.VolunteerManagement.Domain.Entities.Pet", b =>
                {
                    b.HasOne("PetFamily.VolunteerManagement.Domain.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("PetFamily.SharedKernel.ValueObjects.RequisiteList", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Requisites")
                                .IsRequired()
                                .HasColumnType("jsonb")
                                .HasColumnName("requisites");

                            b1.HasKey("PetId");

                            b1.ToTable("pets", "volunteers");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");
                        });

                    b.OwnsOne("PetFamily.VolunteerManagement.Domain.ValueObjects.PetPhotoList", "PetPhotos", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("PetPhotos")
                                .IsRequired()
                                .HasColumnType("jsonb")
                                .HasColumnName("photos");

                            b1.HasKey("PetId");

                            b1.ToTable("pets", "volunteers");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");
                        });

                    b.Navigation("PetPhotos");

                    b.Navigation("Requisites");
                });

            modelBuilder.Entity("PetFamily.VolunteerManagement.Domain.Volunteer", b =>
                {
                    b.OwnsOne("PetFamily.SharedKernel.ValueObjects.SocialNetworkList", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("SocialNetworks")
                                .IsRequired()
                                .HasColumnType("jsonb")
                                .HasColumnName("social_networks");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers", "volunteers");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");
                        });

                    b.OwnsOne("PetFamily.SharedKernel.ValueObjects.RequisiteList", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Requisites")
                                .IsRequired()
                                .HasColumnType("jsonb")
                                .HasColumnName("requisites");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers", "volunteers");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");
                        });

                    b.Navigation("Requisites");

                    b.Navigation("SocialNetworks");
                });

            modelBuilder.Entity("PetFamily.VolunteerManagement.Domain.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
