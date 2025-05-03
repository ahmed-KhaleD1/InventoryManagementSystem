using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Infrastructure.Data;
using InventoryManagement.Infrastructure.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.CQRs.Transactions.Commands
{
    public class AddStockHandler : IRequestHandler<AddStockCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public AddStockHandler(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            Product product = await _context.Products.FindAsync(request.ProductId, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

            product.Quantity += request.Quantity;

            Transaction transaction = new Transaction
            {
                ProductId = request.ProductId,
                TransactionType = TransactionType.Add,
                Quantity = request.Quantity,
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            if (product.Quantity <= product.LowStockThreshold)
            {
                await _notificationService.SendLowStockNotificationAsync(
                    product.Id, product.Name, product.Quantity, product.LowStockThreshold);
            }
        }
    }
}
