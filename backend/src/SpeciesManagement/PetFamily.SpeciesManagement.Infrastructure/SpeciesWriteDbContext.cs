using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PetFamily.SpeciesManagement.Infrastructure
{
    public class SpeciesWriteDbContext(IConfiguration configuration) : DbContext
    {
        private const string DATABASE = "Database";

        public DbSet<Domain.Species> Species { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention()
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration.GetConnectionString(DATABASE));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("species");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
