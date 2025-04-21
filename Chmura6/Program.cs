using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PWC.Common.Domain.Bus;
using PWC.Infra.Bus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// Dodanie kontroler�w
builder.Services.AddControllers();

// Dodanie Swaggera
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mistrz Gry API",
        Version = "v1",
        Description = "API dla zarz�dzania gr� RPG"
    });
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Rejestracja zale�no�ci (np. IEventBus)
builder.Services.AddSingleton<IEventBus, RabbitMQBus>();

var app = builder.Build();

// W��czenie Swaggera
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mistrz Gry API v1");
    options.RoutePrefix = string.Empty; // Ustawienie Swagger UI na stronie g��wnej
});

// Middleware obs�uguj�ce logi
app.UseSerilogRequestLogging();

// Mapowanie kontroler�w
app.MapControllers();

app.Run();
