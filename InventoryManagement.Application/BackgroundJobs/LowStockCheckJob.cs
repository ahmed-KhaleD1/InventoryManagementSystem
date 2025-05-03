using InventoryManagement.Infrastructure.Data;
using InventoryManagement.Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.BackgroundJobs
{
    public class LowStockCheckJob
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public LowStockCheckJob(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task Execute()
        {
            var lowStockProducts = await _context.Products
                .Where(p => p.Quantity <= p.LowStockThreshold)
                .ToListAsync();

            foreach (var product in lowStockProducts)
            {
                await _notificationService.SendLowStockNotificationAsync(product.Id, product.Name, product.Quantity, product.LowStockThreshold);
            }
        }
    }
}
