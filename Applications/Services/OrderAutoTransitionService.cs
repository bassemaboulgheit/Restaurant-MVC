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
    public class OrderAutoTransitionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderAutoTransitionService(IServiceProvider serviceProvider)
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
                        var orderRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Order>>();
                        var orderItemRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<OrderItem>>();

                        var orders = await orderRepository.GetAll(o => o.OrderItems);

                        foreach (var order in orders)
                        {
                            if (order.IsDeleted)
                                continue;

                            var timeSinceUpdate = DateTime.Now - (order.LastUpdated ?? order.OrderDate);

                            if (order.OrderStatus == OrderStatus.Pending && timeSinceUpdate.TotalMinutes >= 5)
                            {
                                order.OrderStatus = OrderStatus.Preparing;
                                order.LastUpdated = DateTime.Now;
                                await orderRepository.Update(order);
                                await orderRepository.Save();

                                System.Diagnostics.Debug.WriteLine($"Order {order.Id} transitioned to Preparing");
                            }

                            if (order.OrderStatus == OrderStatus.Preparing &&
                                order.OrderItems != null && order.OrderItems.Count > 0)
                            {
                                var maxPrepTime = order.OrderItems.Max(oi => oi.MenuItem?.PreparationTime ?? 0);

                                if (timeSinceUpdate.TotalMinutes >= maxPrepTime)
                                {
                                    order.OrderStatus = OrderStatus.Ready;
                                    order.LastUpdated = DateTime.Now;
                                    await orderRepository.Update(order);
                                    await orderRepository.Save();

                                    System.Diagnostics.Debug.WriteLine($"Order {order.Id} transitioned to Ready");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in OrderAutoTransitionService: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}