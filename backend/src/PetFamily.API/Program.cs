using PetFamily.API;
using PetFamily.Infrastructure;
using PetFamily.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices()
    .AddInfrastructure()
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
