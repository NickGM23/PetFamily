
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.BackgroundServices;
using PetFamily.Core.Database;
using PetFamily.Core.FileProvider;
using PetFamily.Core.MessageQueues;
using PetFamily.Core.Messaging;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using PetFamily.VolunteerManagement.Infrastructure.Files;
using PetFamily.VolunteerManagement.Infrastructure.Options;
using PetFamily.VolunteerManagement.Infrastructure.Providers;

namespace PetFamily.VolunteerManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteerInfrastructure(this IServiceCollection collection,
            IConfiguration configuration)
        {
            collection.AddScoped<VolunteersWriteDbContext>();
            collection.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>();
            collection.AddScoped<IVolunteersRepository, VolunteersRepository>();

            collection.AddScoped<IVolunteerUnitOfWork, VolunteerUnitOfWork>();
            collection.AddMinioService(configuration);
            collection.AddScoped<IFilesCleanerService, FilesCleanerService>();

            collection.AddHostedService<FilesCleanerBackgroundService>();

            collection.AddSingleton<IMessageQueue<IEnumerable<Core.FileProvider.FileInfo>>, 
                InMemoryMessageQueue<IEnumerable<Core.FileProvider.FileInfo>>>();

            collection.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return collection;
        }

        private static IServiceCollection AddMinioService(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioOptions>(
                configuration.GetSection(MinioOptions.MINIO));

            services.AddMinio(options =>
            {
                var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                                   ?? throw new ApplicationException("Missing minio configuration");

                options.WithEndpoint(minioOptions.Endpoint);
                options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
                options.WithSSL(minioOptions.WithSSL);
            });

            services.AddScoped<IFileProvider, MinioProvider>();

            return services;
        }
    }
}
