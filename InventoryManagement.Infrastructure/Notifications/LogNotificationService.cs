using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Notifications
{
    public class LogNotificationService : INotificationService
    {
        private readonly ILogger<LogNotificationService> _logger;

        public LogNotificationService(ILogger<LogNotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendLowStockNotificationAsync(int productId, string productName, int quantity, int threshold)
        {
            _logger.LogWarning($"Low stock alert: Product {productName} (ID: {productId}) has {quantity} units, below threshold {threshold}.");
            await Task.CompletedTask;
        }
    }
}
