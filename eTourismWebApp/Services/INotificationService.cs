using eTourismWebApp.Components.Pages;

namespace eTourismWebApp.Services
{
    public interface INotificationService
    {
        event Action<Notificari.Notification>? OnNotificationReceived;
        
        Task<List<Notificari.Notification>> GetNotificationsAsync();
        Task UpdateNotificationAsync(Notificari.Notification notification);
        Task DeleteNotificationAsync(string notificationId);
        Task ClearAllNotificationsAsync();
        Task SendNotificationAsync(Notificari.Notification notification);
    }
}
