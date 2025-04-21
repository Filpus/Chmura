
using BazaBroni.Domain.Entities;
using BazaBroni.Domain.Events;
using BazaBroni.Infrastructure.EventHandlers;
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

    // Rejestracja SupabaseService dla modelu Character
    builder.Services.AddSingleton<SupabaseService<Weapon>>();

    // Rejestracja handlera dla zdarzenia TakeCharacterEvent
    builder.Services.AddScoped<TakeWeaponHandler>();

    var app = builder.Build();

    // Subskrypcja zdarzenia TakeCharacterEvent
    var eventBus = app.Services.GetRequiredService<IEventBus>();
    await eventBus.Subscribe<TakeWeaponEvent, TakeWeaponHandler>();

    app.MapGet("/", () => "Hello World!");

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