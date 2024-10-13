
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Dtos;
using PetFamily.VolunteerManagement.Application;

namespace PetFamily.VolunteerManagement.Infrastructure
{
    public class VolunteersReadDbContext(IConfiguration configuration) : DbContext, IVolunteersReadDbContext
    {
        private const string DATABASE = "Database";

        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);

            modelBuilder.Entity<VolunteerDto>().HasQueryFilter(v => !v.IsDeleted);
            modelBuilder.Entity<PetDto>().HasQueryFilter(v => !v.IsDeleted);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
