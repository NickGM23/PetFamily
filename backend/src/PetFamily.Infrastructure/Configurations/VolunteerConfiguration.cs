using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersManagement;
using PetFamily.Infrastructure.Extensions;

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

            builder.ComplexProperty(v => v.YearsExperience, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .HasColumnName("years_experience");
            });

            builder.ComplexProperty(v => v.PhoneNumber, vb =>
            {
                vb.Property(vp => vp.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PHONENUMBER_LENGHT)
                    .HasColumnName("phone_number");
            });

            builder.OwnsOne(v => v.Requisites, vb =>
            {
                vb.Property(r => r.Requisites)
                    .ValueObjectsCollectionJsonConversion(
                        r => new RequisiteDto(r.Name, r.Description),
                        dto => Requisite.Create(dto.Name, dto.Description).Value)
                    .HasColumnName("requisites");
            });

            builder.OwnsOne(v => v.SocialNetworks, sb =>
            {
                sb.Property(s => s.SocialNetworks)
                    .ValueObjectsCollectionJsonConversion(
                        sn => new SocialNetworkDto(sn.Link, sn.Name),
                        dto => SocialNetwork.Create(dto.Link, dto.Name).Value)
                    .HasColumnName("social_networks");
            });

            //builder.OwnsOne(v => v.SocialNetworks, vb =>
            //{
            //    vb.ToJson();

            //    vb.OwnsMany(vs => vs.SocialNetworks, vsb =>
            //    {
            //        vsb.Property(r => r.Name)
            //        .IsRequired()
            //        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            //        vsb.Property(r => r.Link)
            //        .IsRequired()
            //        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

            //    });
            //});

            //builder.OwnsOne(v => v.Requisites, vb =>
            //{
            //    vb.ToJson();

            //    vb.OwnsMany(vr => vr.Requisites, vrb =>
            //    {
            //        vrb.Property(r => r.Name)
            //        .IsRequired()
            //        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            //        vrb.Property(r => r.Description)
            //        .IsRequired()
            //        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

            //    });
            //});

            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("volunteer_id");

            builder.Navigation(v => v.Pets).AutoInclude();

            builder.Property<bool>("_isDeleted")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("is_deleted");
        }
    }
}
