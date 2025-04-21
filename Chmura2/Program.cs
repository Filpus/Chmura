using BazaPostaci.Domain.Entities;
using BazaPostaci.Domain.Events;
using BazaPostaci.Infrastructure.EventHandler;
using MediatR;
using PWC.Common.Domain.Bus;
using PWC.Infra.Bus;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Dodanie Serilog jako globalnego loggera
    builder.Host.UseSerilog();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    // Rejestracja RabbitMQBus jako Event Bus
    builder.Services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
    {
        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
        return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
    });

    builder.Services.AddScoped<TakeCharacterHandler>();
    // Rejestracja SupabaseService dla modelu Character
    builder.Services.AddSingleton<SupabaseService<Character>>();



    var app = builder.Build();

    // Subskrypcja zdarzenia TakeCharacterEvent
    var eventBus = app.Services.GetRequiredService<IEventBus>();
    await eventBus.Subscribe<TakeCharacterEvent, TakeCharacterHandler>();

    app.MapGet("/", () => "Baza Postaci!");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}