using System.Text.Json;
using Notification.Core.Models.Messages;
using Notification.Core.Models.Requests;
using Notification.Core.Models.Responses;
using StackExchange.Redis;

namespace Notification.Api.Services;

public class NotificationService(IHttpClientFactory httpClientFactory, IConnectionMultiplexer connectionMultiplexer) {
    public async Task<CreateNotificationResponse> CreateNotificationAsync(CreateNotificationRequest request) {
        using var client = httpClientFactory.CreateClient("notification-store");

        var response = await client.PostAsJsonAsync("/store/notification", new StoreNotificationRequest
            { Title = request.Title, Message = request.Message });
        
        response.EnsureSuccessStatusCode();
        
        var storeNotificationResponse = await response.Content.ReadFromJsonAsync<StoreNotificationResponse>();

        if (!request.Publish)
        {
            return new CreateNotificationResponse
            {
                Id = storeNotificationResponse.Id
            };
        }

        var db = connectionMultiplexer.GetDatabase();

        var publishNotification = new PublishNotification
        {
            Title = request.Title,
            Message = request.Message
        };

        var json = JsonSerializer.Serialize(publishNotification);
        await db.PublishAsync("notifications", json);

        return new CreateNotificationResponse
        {
            Id = Guid.NewGuid()
        };
    }
}