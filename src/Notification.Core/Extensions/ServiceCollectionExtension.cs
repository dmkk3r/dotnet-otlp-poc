using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Notification.Core.Extensions;

public static class ServiceCollectionExtension {
    public static IServiceCollection AddOpenTelemetryConfig(this IServiceCollection services, ResourceBuilder resourceBuilder, IConfiguration configuration) {
        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
                tracerProviderBuilder.SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddSource("Notification.Publisher.Services.NotificationPublisher")
                    .AddOtlpExporter(exporterOptions => {
                        var otlpEndpoint = configuration["OTLP_ENDPOINT"];

                        if (string.IsNullOrEmpty(otlpEndpoint)) return;

                        exporterOptions.Endpoint = new Uri(otlpEndpoint);
                        exporterOptions.Protocol = OtlpExportProtocol.Grpc;
                    }))
            .WithMetrics(meterProviderBuilder => {
                meterProviderBuilder.SetResourceBuilder(resourceBuilder);

                meterProviderBuilder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter("Notification.Api.NotificationMeter");

                meterProviderBuilder.AddOtlpExporter(exporterOptions => {
                    var otlpEndpoint = configuration["OTLP_ENDPOINT"];

                    if (string.IsNullOrEmpty(otlpEndpoint)) return;

                    exporterOptions.Endpoint = new Uri(otlpEndpoint);
                    exporterOptions.Protocol = OtlpExportProtocol.Grpc;
                });
            });

        return services;
    }

    public static ILoggingBuilder AddOpenTelemetryConfig(this ILoggingBuilder builder, ResourceBuilder resourceBuilder, IConfiguration configuration) {
        builder.AddOpenTelemetry(options => {
            options.SetResourceBuilder(resourceBuilder);

            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;

            options.AddOtlpExporter(exporterOptions => {
                var otlpEndpoint = configuration["OTLP_ENDPOINT"];

                if (string.IsNullOrEmpty(otlpEndpoint)) return;

                exporterOptions.Endpoint = new Uri(otlpEndpoint);
                exporterOptions.Protocol = OtlpExportProtocol.Grpc;
            });
        });

        return builder;
    }
}