﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasConversion(id => id.Value,
                value => VolunteerId.Create(value));

            builder.ComplexProperty(v => v.FullName, nameBuilder =>
            {
                nameBuilder.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("first_name"); 
                nameBuilder.Property(fn => fn.Patronymic)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("patronymic"); 
                nameBuilder.Property(fn => fn.LastName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("last_name"); ;
            });

            builder.ComplexProperty(v => v.Email, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("email");
            });

            builder.ComplexProperty(v => v.Description, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .HasColumnName("description");
            });

            builder.Property(v => v.YearsExperience)
                .IsRequired();

            builder.ComplexProperty(v => v.PhoneNumber, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT)
                    .HasColumnName("phone_number");
            });

            builder.OwnsOne(v => v.SocialNetworks, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(vs => vs.SocialNetworks, vsb =>
                {
                    vsb.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                    vsb.Property(r => r.Link)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                });
            });

            builder.OwnsOne(v => v.Requisites, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(vr => vr.Requisites, vrb =>
                {
                    vrb.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                    vrb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                });
            });

            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("volunteer_id");

            builder.Navigation(v => v.Pets).AutoInclude();
        }
    }
}
