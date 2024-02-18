using StackExchange.Redis;
using System.Text.Json;
using Notification.Core.Models.Messages;

namespace Notification.Publisher.Services
{
    public class PublisherHostedServices : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public PublisherHostedServices(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _connectionMultiplexer.GetSubscriber();
            subscriber.Subscribe("notifications", (channel, message) =>
            {
                var notification = JsonSerializer.Deserialize<PublishNotification>(message);
                // Hier können Sie den Code hinzufügen, um mit der empfangenen Nachricht zu interagieren
            });

            // Da das Abonnement asynchron ist, geben wir hier einfach ein abgeschlossenes Task zurück
            return Task.CompletedTask;
        }
    }
}