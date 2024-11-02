using Serilog.Events;
using Serilog;
using PetFamily.Web.Middlewares;
using PetFamily.Web.Extensions;
using PetFamily.Web;
using PetFamily.VolunteerManagement.Presentation;
using PetFamily.SpeciesManagement.Infrastructure;
using PetFamily.VolunteerManagement.Infrastructure;
using PetFamily.SpeciesManagement.Presentation;
using PetFamily.Accounts.Presentation;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Accounts.Infrastructure.DbContexts;

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
    .AddSpeciesModule(builder.Configuration)
    .AddVolunteerModule(builder.Configuration)
    .AddAccountsModule(builder.Configuration)
    .AddWeb()
    .AddHttpLogging(u =>
    {
        u.CombineLogs = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrations<SpeciesWriteDbContext>();
    await app.ApplyMigrations<VolunteersWriteDbContext>();
    await app.ApplyMigrations<AccountsWriteDbContext>();
}

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

var accountSeeder = app.Services.GetRequiredService<AccountsSeeder>();
await accountSeeder.SeedAsync();

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
