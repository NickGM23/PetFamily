using PetFamily.API.Validation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetFamily.API
{
    public static class Inject
    {
        public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen();
            serviceCollection.AddControllers();

            serviceCollection.AddFluentValidationAutoValidation(configuration =>
            {
                configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
            });

            return serviceCollection;
        }
    }
}
