using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
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
    public class TransactionHistoryHandler : IRequestHandler<TransactionHistoryQuery, PagedResult<TransactionDto>>
    {
        private readonly ApplicationDbContext _context;

        public TransactionHistoryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TransactionDto>> Handle(TransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Transaction> query = _context.Transactions.AsQueryable();

            if (request.ProductId.HasValue)
                query = query.Where(t => t.ProductId == request.ProductId.Value);
            if (request.StartDate.HasValue)
                query = query.Where(t => t.Date >= request.StartDate.Value);
            if (request.EndDate.HasValue)
                query = query.Where(t => t.Date <= request.EndDate.Value);

            int total = await query.CountAsync(cancellationToken);
            List<TransactionDto> items = await query
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    ProductId = t.ProductId,
                    TransactionType = t.TransactionType,
                    Quantity = t.Quantity,
                    Date = t.Date,
                    SourceWarehouseId = t.SourceWarehouseId,
                    DestinationWarehouseId = t.DestinationWarehouseId
                })
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<TransactionDto>
            {
                Items = items,
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
