using StackExchange.Redis;
using System.Text.Json;
using Notification.Core.Models.Messages;

namespace Notification.Publisher.Services;

public class PublisherHostedServices(IConnectionMultiplexer connectionMultiplexer, NotificationPublisher notificationPublisher) : BackgroundService {
    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        var subscriber = connectionMultiplexer.GetSubscriber();
        subscriber.Subscribe("notifications", RedisNotificationHandler);

        return Task.CompletedTask;

        async void RedisNotificationHandler(RedisChannel channel, RedisValue message) {
            var notification = JsonSerializer.Deserialize<PublishNotification>(message);
            await notificationPublisher.PublishNotificationAsync(notification);
        }
    }
}