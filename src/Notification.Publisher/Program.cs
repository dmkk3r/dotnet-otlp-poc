using Notification.Core.Extensions;
using Notification.Publisher.Services;
using OpenTelemetry.Resources;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService(serviceName: "notification-publisher",
        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
        serviceInstanceId: Environment.MachineName);

builder.Services.AddOpenTelemetryConfig(resourceBuilder, builder.Configuration);
builder.Logging.AddOpenTelemetryConfig(resourceBuilder, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConnectionMultiplexer redisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton(redisConnectionMultiplexer);
builder.Services.AddStackExchangeRedisCache(options => options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer));

builder.Services.AddHostedService<PublisherHostedServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/weatherforecast", () => { })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();