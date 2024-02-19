using Notification.Api.Services;
using Notification.Core.Extensions;
using Notification.Core.Models.Requests;
using OpenTelemetry.Resources;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService(serviceName: "notification-api",
        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
        serviceInstanceId: Environment.MachineName);

builder.Services.AddOpenTelemetryConfig(resourceBuilder, builder.Configuration);
builder.Logging.AddOpenTelemetryConfig(resourceBuilder, builder.Configuration);

builder.Services.AddScoped<NotificationService>();
builder.Services.AddSingleton<NotificationMetrics>();

builder.Services.AddHttpClient("notification-store", c => {
    c.BaseAddress = new Uri("http://localhost:5090");
});

IConnectionMultiplexer redisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton(redisConnectionMultiplexer);
builder.Services.AddStackExchangeRedisCache(options => options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var notificationGroup = app.MapGroup("/notifications");

notificationGroup.MapPost("/", async (CreateNotificationRequest request, NotificationService notificationService) => {
        await notificationService.CreateNotificationAsync(request);
    })
    .WithName("CreateNotification")
    .WithOpenApi();

app.Run();