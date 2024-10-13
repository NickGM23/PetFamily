using PetFamily.Web.Validation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetFamily.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection serviceCollection)
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
