
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteerManagement.Domain;

namespace PetFamily.VolunteerManagement.Infrastructure
{
    public class VolunteersWriteDbContext(IConfiguration configuration) : DbContext
    {
        private const string DATABASE = "Database";

        public DbSet<Volunteer> Volunteers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention()
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration.GetConnectionString(DATABASE));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
