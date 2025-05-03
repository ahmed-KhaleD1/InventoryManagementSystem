using InventoryManagement.Application.DTOs;
using InventoryManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.CQRs.Transactions.Queries
{
    public class LowStockReportHandler : IRequestHandler<LowStockReportQuery, PagedResult<ProductDto>>
    {
        private readonly ApplicationDbContext _context;

        public LowStockReportHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductDto>> Handle(LowStockReportQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ProductDto> query = _context.Products
                .Where(p => p.Quantity <= p.LowStockThreshold)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    LowStockThreshold = p.LowStockThreshold
                });

            int total = await query.CountAsync(cancellationToken);
            List<ProductDto> items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ProductDto>
            {
                Items = items,
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
