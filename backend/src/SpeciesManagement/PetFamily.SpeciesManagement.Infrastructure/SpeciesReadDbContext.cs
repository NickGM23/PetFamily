
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Dtos;
using PetFamily.SpeciesManagement.Application;

namespace PetFamily.SpeciesManagement.Infrastructure
{
    public class SpeciesReadDbContext(IConfiguration configuration) : DbContext, ISpeciesReadDbContext
    {
        private const string DATABASE = "Database";

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
        public IQueryable<BreedDto> Breeds => Set<BreedDto>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention()
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration.GetConnectionString(DATABASE))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        private ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                    .AddConsole();
            });
        }
    }
}
