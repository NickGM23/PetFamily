
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.Get.GetFile;
using PetFamily.Application.Files.Get.GetFiles;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;
using PetFamily.Application.Volunteers.UploadFilesToPet;
using PetFamily.Application.Abstractions;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<CreateVolunteerHandler>();
            serviceCollection.AddScoped<UpdateVolunteerMainInfoHandler>();
            serviceCollection.AddScoped<DeleteVolunteerHandler>();
            serviceCollection.AddScoped<UpdateRequisitesHandler>();
            serviceCollection.AddScoped<UpdateSocialNetworksHandler>();

            serviceCollection.AddScoped<UploadFileHandler>();
            serviceCollection.AddScoped<RemoveFileHandler>();
            serviceCollection.AddScoped<GetFileHandler>();
            serviceCollection.AddScoped<GetFilesHandler>();

            serviceCollection.AddScoped<AddPetHandler>();
            serviceCollection.AddScoped<UploadFilesToPetHandler>();

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
