namespace Notification.Store.Entities;

public class NotificationEntity {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}