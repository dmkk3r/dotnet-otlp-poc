namespace Notification.Core.Models;

public class Notification {
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
}