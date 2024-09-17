
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.Get.GetFile;
using PetFamily.Application.Files.Get.GetFiles;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

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

            serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
            return serviceCollection;
        }
    }
}
