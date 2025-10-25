using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Applications.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;

namespace Applications.Services
{
    public class DailyAvailabilityService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DailyAvailabilityService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var menuItemRepository = scope.ServiceProvider
                            .GetRequiredService<IGenericRepository<MenuItem>>();

                        // Get all menu items
                        var items = await menuItemRepository.GetAll();

                        foreach (var item in items)
                        {
                            // Check if it's a new day
                            if (item.LastResetDate.Date < DateTime.Today)
                            {
                                // Reset daily count
                                item.DailyOrderCount = 0;
                                item.LastResetDate = DateTime.Today;

                                await menuItemRepository.Update(item);
                                System.Diagnostics.Debug.WriteLine(
                                    $"Reset daily count for item {item.Id}: {item.Name}");
                            }

                            // Mark as unavailable if >50 orders today
                            if (item.DailyOrderCount >= 50)
                            {
                                item.Quantity = 0; // Set quantity to 0 to disable
                                await menuItemRepository.Update(item);
                                System.Diagnostics.Debug.WriteLine(
                                    $"Item {item.Id}: {item.Name} is now unavailable (50+ orders)");
                            }
                        }

                        await menuItemRepository.Save();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Error in DailyAvailabilityService: {ex.Message}");
                }

                // Check every hour at midnight
                var now = DateTime.Now;
                var nextMidnight = DateTime.Today.AddDays(1);
                var timeUntilMidnight = nextMidnight - now;

                // Also check every 30 minutes during the day
                var checkInterval = timeUntilMidnight > TimeSpan.Zero
                    ? timeUntilMidnight
                    : TimeSpan.FromMinutes(30);

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}