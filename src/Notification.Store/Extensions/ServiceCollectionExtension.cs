using Microsoft.EntityFrameworkCore;

namespace Notification.Store.Extensions;

public static class ServiceCollectionExtension {
    public static IServiceCollection AddPostgres(
        this IServiceCollection services, IConfiguration configuration) {
        var postgresHost = configuration["POSTGRES_HOST"];
        var postgresPort = configuration["POSTGRES_PORT"];
        var postgresDatabase = configuration["POSTGRES_DB"];
        var postgresUser = configuration["POSTGRES_USER"];
        var postgresPassword = configuration["POSTGRES_PASSWORD"];

        var connectionString =
            $"Host={postgresHost};Port={postgresPort};Database={postgresDatabase};Username={postgresUser};Password={postgresPassword}";

        services.AddDbContext<NotificationStoreContext>(options => {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}