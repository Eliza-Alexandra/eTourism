using eTourismWebApp.Components.Pages;

namespace eTourismWebApp.Services
{
    public interface IDashboardService
    {
        Task<Statistici.DashboardStats> GetStatsAsync();
        Task<List<Statistici.RecentActivity>> GetRecentActivityAsync();
    }
}
