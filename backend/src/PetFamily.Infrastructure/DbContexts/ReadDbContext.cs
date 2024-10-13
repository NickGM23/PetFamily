﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Database;
using PetFamily.Core.Dtos;

namespace PetFamily.Infrastructure.DbContexts
{
    public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
    {
        private const string DATABASE = "Database";

        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();

        public IQueryable<PetDto> Pets => Set<PetDto>();

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

        public IQueryable<BreedDto> Breeds => Set<BreedDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
