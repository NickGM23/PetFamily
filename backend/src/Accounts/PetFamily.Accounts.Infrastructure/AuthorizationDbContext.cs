
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure
{
    public class AuthorizationDbContext(IConfiguration configuration)
        : IdentityDbContext<User, Role, Guid>
    {
        private const string DATABASE = "Database";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention()
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration.GetConnectionString(DATABASE));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .ToTable("users");

            modelBuilder.Entity<Role>()
                .ToTable("roles");

            modelBuilder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("user_claims");

            modelBuilder.Entity<IdentityUserToken<Guid>>()
                .ToTable("user_tokens");

            modelBuilder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("user_logins");

            modelBuilder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("role_claims");

            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .ToTable("user_roles");
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
