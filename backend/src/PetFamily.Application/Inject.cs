
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
            serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
            return serviceCollection;
        }
    }
}
