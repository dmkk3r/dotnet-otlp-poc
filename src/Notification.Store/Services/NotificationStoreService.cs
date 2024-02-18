using Notification.Core.Models.Requests;
using Notification.Core.Models.Responses;
using Notification.Store.Entities;

namespace Notification.Store.Services;

public class NotificationStoreService(NotificationStoreContext notificationStoreContext) {
    public async Task<StoreNotificationResponse> StoreNotificationAsync(StoreNotificationRequest request) {
        var notification = new NotificationEntity()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        notificationStoreContext.Notifications.Add(notification);
        await notificationStoreContext.SaveChangesAsync();

        return new StoreNotificationResponse
        {
            Id = notification.Id
        };
    }
}