using Notification.Core.Extensions;
using Notification.Core.Models.Requests;
using Notification.Store.Extensions;
using Notification.Store.Services;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService(serviceName: "notification-store",
        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
        serviceInstanceId: Environment.MachineName);

builder.Services.AddOpenTelemetryConfig(resourceBuilder, builder.Configuration);
builder.Logging.AddOpenTelemetryConfig(resourceBuilder, builder.Configuration);

builder.Services.AddPostgres(builder.Configuration);

builder.Services.AddScoped<NotificationStoreService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var notificationGroup = app.MapGroup("/store");

notificationGroup.MapPost("/notification", async (StoreNotificationRequest request, NotificationStoreService notificationStoreService) =>
        await notificationStoreService.StoreNotificationAsync(request)
    )
    .WithName("StoreNotification")
    .WithOpenApi();

app.Run();