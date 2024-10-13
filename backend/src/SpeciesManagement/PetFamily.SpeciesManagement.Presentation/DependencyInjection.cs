
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SpeciesManagement.Application;
using PetFamily.SpeciesManagement.Contract;
using PetFamily.SpeciesManagement.Infrastructure;

namespace PetFamily.SpeciesManagement.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSpeciesModule(
             this IServiceCollection collection, IConfiguration configuration)
        {
            return collection.AddScoped<ISpeciesContract, SpeciesContract>()
                .AddSpeciesApplication()
                .AddSpeciesInfrastructure(configuration);
        }
    }
}
