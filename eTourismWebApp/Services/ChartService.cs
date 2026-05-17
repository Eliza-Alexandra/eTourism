namespace eTourismWebApp.Services
{
    public class ChartService : IChartService
    {
        private readonly Random _random = new();

        public async Task<ChartData> GetActivityChartDataAsync()
        {
            await Task.Delay(100);

            var chartData = new ChartData
            {
                Title = "Activitate Zilnică"
            };

            // Generează date pentru ultimele 7 zile
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Now.AddDays(-i);
                chartData.DataPoints.Add(new DataPoint
                {
                    Index = 6 - i,
                    Label = date.ToString("dd MMM"),
                    Value = _random.Next(40, 100)
                });
            }

            return chartData;
        }

        public async Task<ChartData> GetUserDistributionDataAsync()
        {
            await Task.Delay(50);

            var chartData = new ChartData
            {
                Title = "Distribuție Utilizatori"
            };

            var userTypes = new[]
            {
                new { Name = "Admin", Count = 5 },
                new { Name = "Moderatori", Count = 12 },
                new { Name = "Utilizatori", Count = 850 },
                new { Name = "Vizitatori", Count = 383 }
            };

            int index = 0;
            foreach (var userType in userTypes)
            {
                chartData.DataPoints.Add(new DataPoint
                {
                    Index = index++,
                    Label = userType.Name,
                    Value = userType.Count
                });
            }

            return chartData;
        }

        public async Task<ChartData> GetPerformanceDataAsync()
        {
            await Task.Delay(75);

            var chartData = new ChartData
            {
                Title = "Metrici Performanță"
            };

            var metrics = new[]
            {
                new { Name = "CPU", Value = 67 },
                new { Name = "Memory", Value = 82 },
                new { Name = "Disk", Value = 45 },
                new { Name = "Network", Value = 23 },
                new { Name = "Database", Value = 58 }
            };

            int index = 0;
            foreach (var metric in metrics)
            {
                chartData.DataPoints.Add(new DataPoint
                {
                    Index = index++,
                    Label = metric.Name,
                    Value = metric.Value
                });
            }

            return chartData;
        }
    }
}
