using PetFamily.API;
using PetFamily.Infrastructure;
using PetFamily.Application;
using PetFamily.API.Extensions;
using Serilog.Events;
using Serilog;
using PetFamily.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services
    .AddApiServices()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
