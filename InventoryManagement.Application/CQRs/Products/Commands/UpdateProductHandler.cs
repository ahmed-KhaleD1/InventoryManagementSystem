using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.CQRs.Products.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProductHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _context.Products.FindAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Product name is required.");
            if (request.Quantity < 0 || request.Price < 0 || request.LowStockThreshold < 0)
                throw new ArgumentException("Quantity, Price, and LowStockThreshold must be non-negative.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Quantity = request.Quantity;
            product.Price = request.Price;
            product.LowStockThreshold = request.LowStockThreshold;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
