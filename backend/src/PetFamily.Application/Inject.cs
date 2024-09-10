﻿
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<CreateVolunteerHandler>();
            serviceCollection.AddValidatorsFromAssembly(typeof(Inject).Assembly);
            return serviceCollection;
        }
    }
}
