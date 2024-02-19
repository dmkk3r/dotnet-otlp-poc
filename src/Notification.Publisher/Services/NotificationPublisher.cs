using System.Diagnostics;
using Notification.Core.Models.Messages;

namespace Notification.Publisher.Services;

public class NotificationPublisher(ILogger<NotificationPublisher> logger) {
    private static readonly ActivitySource NotificationActivity = new("Notification.Publisher.Services.NotificationPublisher");

    public Task PublishNotificationAsync(PublishNotification notification) {
        var activityContext = new ActivityContext(ActivityTraceId.CreateFromString(notification.TraceId), ActivitySpanId.CreateRandom(),
            ActivityTraceFlags.Recorded);

        using var activity = NotificationActivity.StartActivity("PublishNotification", ActivityKind.Consumer, activityContext);

        activity?.SetTag("notification.title", notification.Title);
        activity?.SetTag("notification.message", notification.Message);

        activity?.AddEvent(new ActivityEvent("Publishing notification"));

        logger.LogInformation("Publishing notification. Title: {NotificationTitle}, Message: {NotificationMessage}", notification.Title, notification.Message);

        return Task.CompletedTask;
    }
}