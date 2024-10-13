
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.Get.GetFile;
using PetFamily.Application.Files.Get.GetFiles;
using PetFamily.Application.Files.Upload;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<UploadFileHandler>();
            serviceCollection.AddScoped<RemoveFileHandler>();
            serviceCollection.AddScoped<GetFileHandler>();
            serviceCollection.AddScoped<GetFilesHandler>();

            serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            serviceCollection.Scan(type => type.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            serviceCollection.Scan(type => type.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return serviceCollection;
        }
    }
}
