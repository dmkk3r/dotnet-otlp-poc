using System.Diagnostics.Metrics;

namespace Notification.Api.Services;

public class NotificationMetrics {
    private readonly Counter<int> _notificationCounter;

    public NotificationMetrics(IMeterFactory meterFactory) {
        var meter = meterFactory.Create("Notification.Api.NotificationMeter");

        _notificationCounter = meter.CreateCounter<int>("notification.counter");
    }

    public void IncrementNotificationCounter() {
        _notificationCounter.Add(1);
    }
}