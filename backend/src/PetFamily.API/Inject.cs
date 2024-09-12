using PetFamily.API.Validation;
using Serilog;
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

            serviceCollection.AddSerilog();

            serviceCollection.AddFluentValidationAutoValidation(configuration =>
            {
                configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
            });

            return serviceCollection;
        }
    }
}
