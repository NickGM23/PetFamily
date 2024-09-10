namespace PetFamily.API
{
    public static class Inject
    {
        public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen();
            serviceCollection.AddControllers();

            return serviceCollection;
        }
    }
}
