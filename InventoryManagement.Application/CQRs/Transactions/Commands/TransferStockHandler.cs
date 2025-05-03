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
    public class TransferStockHandler : IRequestHandler<TransferStockCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public TransferStockHandler(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task Handle(TransferStockCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            if (request.SourceWarehouseId == request.DestinationWarehouseId)
                throw new ArgumentException("Source and destination warehouses must be different.");

            Product product = await _context.Products.FindAsync(request.ProductId, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
            Warehouse sourceWarehouse = await _context.Warehouses.FindAsync(request.SourceWarehouseId, cancellationToken)
                ?? throw new KeyNotFoundException($"Source warehouse with ID {request.SourceWarehouseId} not found.");
            Warehouse destinationWarehouse = await _context.Warehouses.FindAsync(request.DestinationWarehouseId, cancellationToken)
                ?? throw new KeyNotFoundException($"Destination warehouse with ID {request.DestinationWarehouseId} not found.");

            if (product.Quantity < request.Quantity)
                throw new InvalidOperationException("warehouse stock is Not Enough.");

            Transaction transaction = new Transaction
            {
                ProductId = request.ProductId,
                TransactionType = TransactionType.Transfer,
                Quantity = request.Quantity,
                Date = DateTime.UtcNow,
                SourceWarehouseId = request.SourceWarehouseId,
                DestinationWarehouseId = request.DestinationWarehouseId
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
