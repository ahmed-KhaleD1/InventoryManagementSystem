using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Notifications
{
    public interface INotificationService
    {
        Task SendLowStockNotificationAsync(int productId, string productName, int quantity, int threshold);
    }
}
