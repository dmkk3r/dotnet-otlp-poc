using Microsoft.EntityFrameworkCore;
using Notification.Store.Entities;

namespace Notification.Store;

public class NotificationStoreContext(DbContextOptions<NotificationStoreContext> options) : DbContext(options) {
    public DbSet<NotificationEntity> Notifications { get; set; }
}