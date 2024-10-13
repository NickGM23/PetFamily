
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Core.BackgroundServices;
using PetFamily.Core.Database;
using PetFamily.Core.FileProvider;
using PetFamily.Core.MessageQueues;
using PetFamily.Core.Messaging;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.Files;
using PetFamily.Infrastructure.Repositories;
using PetFamily.SpeciesManagement.Application;
using PetFamily.VolunteerManagement.Application;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;

namespace PetFamily.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<WriteDbContext>();

            services.AddScoped<IReadDbContext, ReadDbContext>();

            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IFilesCleanerService, FilesCleanerService>();

            services.AddHostedService<FilesCleanerBackgroundService>();

            services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            return services;
        }
    }
}
