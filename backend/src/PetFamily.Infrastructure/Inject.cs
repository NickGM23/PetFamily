
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Files;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ApplicationDbContext>();

            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMinio(configuration);

            services.AddScoped<IFilesCleanerService, FilesCleanerService>();

            services.AddHostedService<FilesCleanerBackgroundService>();

            services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            return services;
        }
        private static IServiceCollection AddMinio(
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
