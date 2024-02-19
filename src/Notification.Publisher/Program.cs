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

IConnectionMultiplexer redisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton(redisConnectionMultiplexer);
builder.Services.AddStackExchangeRedisCache(options => options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer));

builder.Services.AddHostedService<PublisherHostedServices>();
builder.Services.AddSingleton<NotificationPublisher>();

var app = builder.Build();

app.Run();