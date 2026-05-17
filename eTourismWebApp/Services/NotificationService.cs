using eTourismWebApp.Components.Pages;

namespace eTourismWebApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly List<Notificari.Notification> _notifications = new();
        private readonly Random _random = new();

        public event Action<Notificari.Notification>? OnNotificationReceived;

        public NotificationService()
        {
            Console.WriteLine("NotificationService constructor - Început");
            InitializeSampleNotifications();
            StartSimulation(); 
            Console.WriteLine($"NotificationService constructor - Inițializate {_notifications.Count} notificări");
        }

        public async Task<List<Notificari.Notification>> GetNotificationsAsync()
        {
            try
            {
                Console.WriteLine("NotificationService.GetNotificationsAsync - Început");
                await Task.Delay(50); // Simulare network delay
                var result = _notifications.OrderByDescending(n => n.CreatedAt).ToList();
                Console.WriteLine($"NotificationService.GetNotificationsAsync - Returnat {result.Count} notificări");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare în NotificationService.GetNotificationsAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateNotificationAsync(Notificari.Notification notification)
        {
            await Task.Delay(25);
            
            var existing = _notifications.FirstOrDefault(n => n.Id == notification.Id);
            if (existing != null)
            {
                existing.IsRead = notification.IsRead;
                existing.Title = notification.Title;
                existing.Message = notification.Message;
            }
        }

        public async Task DeleteNotificationAsync(string notificationId)
        {
            await Task.Delay(25);
            _notifications.RemoveAll(n => n.Id == notificationId);
        }

        public async Task ClearAllNotificationsAsync()
        {
            await Task.Delay(100);
            _notifications.Clear();
        }

        public async Task SendNotificationAsync(Notificari.Notification notification)
        {
            await Task.Delay(50);
            
            notification.Id = Guid.NewGuid().ToString();
            notification.CreatedAt = DateTime.Now;
            
            _notifications.Insert(0, notification);
            
            // Trigger real-time event
            OnNotificationReceived?.Invoke(notification);
        }

        private void InitializeSampleNotifications()
        {
            var sampleNotifications = new[]
            {
                new Notificari.Notification
                {
                    Title = "Sistem actualizat cu succes",
                    Message = "eTourism a fost actualizat la versiunea 2.1.0 cu noi funcționalități de securitate.",
                    Type = "Success",
                    Priority = "Medium",
                    Source = "System",
                    CreatedAt = DateTime.Now.AddMinutes(-15)
                },
                new Notificari.Notification
                {
                    Title = "Avertisment securitate",
                    Message = "S-au detectat încercări de login neautorizate de la adresa IP 192.168.1.100.",
                    Type = "Warning",
                    Priority = "High",
                    Source = "Security",
                    CreatedAt = DateTime.Now.AddMinutes(-45)
                },
                new Notificari.Notification
                {
                    Title = "Backup finalizat",
                    Message = "Backup-ul zilnic al bazei de date a fost finalizat cu succes. Dimensiune: 2.3 GB",
                    Type = "Info",
                    Priority = "Low",
                    Source = "Backup Service",
                    CreatedAt = DateTime.Now.AddHours(-2)
                },
                new Notificari.Notification
                {
                    Title = "Eroare conexiune bază de date",
                    Message = "Conexiunea la baza de date a fost pierdută temporar. Sistemul încearcă reconectarea automat.",
                    Type = "Error",
                    Priority = "Critical",
                    Source = "Database",
                    CreatedAt = DateTime.Now.AddHours(-3),
                    IsRead = false
                },
                new Notificari.Notification
                {
                    Title = "Raport săptămânal disponibil",
                    Message = "Raportul de utilizare pentru săptămâna trecută este acum disponibil pentru descărcare.",
                    Type = "Info",
                    Priority = "Medium",
                    Source = "Reports",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    IsRead = true
                },
                new Notificari.Notification
                {
                    Title = "Utilizator nou înregistrat",
                    Message = "Utilizatorul 'john.doe@company.com' și-a creat cont nou în sistem.",
                    Type = "Info",
                    Priority = "Low",
                    Source = "User Management",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    IsRead = true
                }
            };

            _notifications.AddRange(sampleNotifications);
        }

        // Metodă pentru simulare notificări în timp real
        public void StartSimulation()
        {
            var timer = new Timer(async _ =>
            {
                if (_random.Next(1, 10) == 1) // 10% șansă
                {
                    var notifications = new[]
                    {
                        new Notificari.Notification
                        {
                            Title = "Activitate suspectă detectată",
                            Message = "S-au detectat activități neobișnuite în contul utilizatorului 'admin'.",
                            Type = "Warning",
                            Priority = "High",
                            Source = "Security Monitor"
                        },
                        new Notificari.Notification
                        {
                            Title = "Performanță sistem optimă",
                            Message = "Toate serviciile sistemului funcționează în parametri normali.",
                            Type = "Success",
                            Priority = "Low",
                            Source = "System Monitor"
                        },
                        new Notificari.Notification
                        {
                            Title = "Actualizare disponibilă",
                            Message = "O nouă actualizare de securitate este disponibilă pentru instalare.",
                            Type = "Info",
                            Priority = "Medium",
                            Source = "Update Manager"
                        }
                    };

                    var notification = notifications[_random.Next(notifications.Length)];
                    await SendNotificationAsync(notification);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }
    }
}
