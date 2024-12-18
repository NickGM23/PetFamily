﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetFamily.Core.Options;
using PetFamily.Web.Validation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

namespace PetFamily.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddControllers();

            serviceCollection.AddSerilog();

            serviceCollection.AddAuthFieldInSwagger();
            //serviceCollection.AddJwtAuthentication();

            serviceCollection.AddFluentValidationAutoValidation(configuration =>
            {
                configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
            });

            return serviceCollection;
        }

        private static IServiceCollection AddAuthFieldInSwagger(this IServiceCollection collection)
        {
            return collection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        []
                    }
                });
            });
        }

        //private static IServiceCollection AddJwtAuthentication(this IServiceCollection collection)
        //{
        //    collection.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //        .AddJwtBearer(options =>
        //        {
        //            var jwtOptions = collection
        //                .BuildServiceProvider()
        //                .GetRequiredService<IOptions<JwtOptions>>().Value;
        //            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

        //            options.TokenValidationParameters = new TokenValidationParameters()
        //            {
        //                ValidIssuer = jwtOptions.Issuer,
        //                ValidAudience = jwtOptions.Audience,
        //                IssuerSigningKey = issuerSigningKey,
        //                ValidateIssuer = false,
        //                ValidateAudience = true,
        //                ValidateLifetime = false,
        //                ValidateIssuerSigningKey = true,
        //                ClockSkew = TimeSpan.Zero
        //            };
        //        });
        //    return collection;
        //}
    }
}
