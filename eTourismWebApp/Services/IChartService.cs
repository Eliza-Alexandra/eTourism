namespace eTourismWebApp.Services
{
    public interface IChartService
    {
        Task<ChartData> GetActivityChartDataAsync();
        Task<ChartData> GetUserDistributionDataAsync();
        Task<ChartData> GetPerformanceDataAsync();
    }

    public class ChartData
    {
        public List<DataPoint> DataPoints { get; set; } = new();
        public List<LegendItem> Legend { get; set; } = new();
        public string Title { get; set; } = string.Empty;
    }

    public class DataPoint
    {
        public int Index { get; set; }
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public string? Color { get; set; }
    }

    public class LegendItem
    {
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
