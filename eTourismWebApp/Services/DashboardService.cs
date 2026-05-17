using eTourismWebApp.Components.Pages;

namespace eTourismWebApp.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly Random _random = new();

        public DashboardService()
        {
        }

        public async Task<Statistici.DashboardStats> GetStatsAsync()
        {
            // Simulare date - în producție ar veni de la API
            await Task.Delay(100); // Simulare network delay

            return new Statistici.DashboardStats
            {
                TotalUsers = _random.Next(1200, 1500),
                ActiveSessions = _random.Next(80, 120),
                SystemLoad = (decimal)(_random.NextDouble() * 40 + 50), // 50-90%
                AlertCount = _random.Next(0, 8)
            };
        }

        public async Task<List<Statistici.RecentActivity>> GetRecentActivityAsync()
        {
            await Task.Delay(50);

            var activities = new List<Statistici.RecentActivity>();
            var actions = new[] { "Login", "Logout", "Vizualizare cazări", "Vizualizare obiective", "Actualizare Profil" };
            var users = new[] { "Alexandra Gheorghe", "Eliza Cace", "Andreea Timiras", "Ion Popescu", "Andrei Ionescu" };

            for (int i = 0; i < 10; i++)
            {
                activities.Add(new Statistici.RecentActivity
                {
                    UserName = users[_random.Next(users.Length)],
                    Action = actions[_random.Next(actions.Length)],
                    Timestamp = DateTime.Now.AddMinutes(-_random.Next(1, 120))
                });
            }

            return activities.OrderByDescending(a => a.Timestamp).ToList();
        }
    }
}
