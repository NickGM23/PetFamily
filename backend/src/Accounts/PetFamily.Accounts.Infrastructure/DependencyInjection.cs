using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Authorization;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.IdentyManagers;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Core.Options;
using PetFamily.Framework.Authorization;
using System.Text;

namespace PetFamily.Accounts.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountsInfrastructure(
            this IServiceCollection collection, IConfiguration configuration)
        {
            collection.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
            collection.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));

            collection.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<AccountsWriteDbContext>()
                .AddDefaultTokenProviders();

            collection.AddScoped<AccountsWriteDbContext>();

            collection
                .ConfigureIdentityManagers()
                .ConfigureScopedServices()
                .ConfigureAuthorizationHandlers()
                .AddJwtAuthentication();

            return collection;
        }

        private static IServiceCollection ConfigureIdentityManagers(this IServiceCollection collection)
        {
            return collection.AddTransient<ITokenProvider, JwtTokenProvider>()
                .AddSingleton<AccountsSeeder>();
        }

        private static IServiceCollection ConfigureScopedServices(this IServiceCollection collection)
        {
            return collection.AddScoped<IAccountsReadDbContext, AccountsReadDbContext>()
                .AddScoped<RolePermissionManager>()
                .AddScoped<AccountsWriteDbContext>()
                .AddScoped<PermissionManager>()
                .AddScoped<AccountSeederService>()
                .AddScoped<AdminAccountManager>()
                .AddScoped<IAccountsUnitOfWork, AccountsUnitOfWork>()
                .AddScoped<IParticipantAccountManager, ParticipantAccountManager>()
                .AddScoped<IVolunteerAccountManager, VolunteerAccountManager>()
                .AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        }

        private static IServiceCollection ConfigureAuthorizationHandlers(this IServiceCollection collection)
        {
            return collection.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection collection)
        {
            collection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    var jwtOptions = collection
                        .BuildServiceProvider()
                        .GetRequiredService<IOptions<JwtOptions>>().Value;

                    //var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
                    //options.TokenValidationParameters = new TokenValidationParameters()
                    //{
                    //    ValidIssuer = jwtOptions.Issuer,
                    //    ValidAudience = jwtOptions.Audience,
                    //    IssuerSigningKey = issuerSigningKey,
                    //    ValidateIssuer = false,
                    //    ValidateAudience = false,
                    //    ValidateLifetime = false,
                    //    ValidateIssuerSigningKey = true,
                    //    ClockSkew = TimeSpan.Zero
                    //};
                    options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifetime(jwtOptions);
                });
            return collection;
        }
    }
}
