using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.VolunteersManagement;
using PetFamily.Domain.SpeciesManagement;

namespace PetFamily.Infrastructure
{
    public  class ApplicationDbContext(IConfiguration configuration) : DbContext
    {

        private const string DATABASE = "Database";

        public DbSet<Volunteer> Volunteers { get; set; }

        public DbSet<Species> Species { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
