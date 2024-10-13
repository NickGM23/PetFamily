using Serilog.Events;
using Serilog;
using System.Reflection;
using PetFamily.Web.Middlewares;
using PetFamily.Web.Extensions;
using PetFamily.Web;
using PetFamily.VolunteerManagement.Presentation;
using PetFamily.SpeciesManagement.Infrastructure;
using PetFamily.VolunteerManagement.Infrastructure;
using PetFamily.SpeciesManagement.Presentation;

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
    .AddWeb()
    .AddSpeciesModule(builder.Configuration)
    .AddVolunteerModule(builder.Configuration);

var xmlDoc = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "xml");
builder.Services.AddSwaggerGen(options => options.IncludeXmlComments(xmlDoc, true));

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrations<SpeciesWriteDbContext>();
    await app.ApplyMigrations<VolunteersWriteDbContext>();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
