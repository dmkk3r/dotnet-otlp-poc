namespace Notification.Core.Models.Messages;

public class PublishNotification {
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? TraceId { get; set; }
}