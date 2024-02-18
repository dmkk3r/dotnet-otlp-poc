namespace Notification.Core.Models.Requests;

public class CreateNotificationRequest {
    public string? Title { get; set; }
    public string? Message { get; set; }
    public bool Publish { get; set; }
}