using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.BackgroundJobs
{
    public class TransactionArchiveJob
    {
        private readonly ApplicationDbContext _context;

        public TransactionArchiveJob(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Execute()
        {
            DateTime oneYearAgo = DateTime.UtcNow.AddYears(-1);
            List<Transaction> oldTransactions = await _context.Transactions
                .Where(t => t.Date < oneYearAgo)
                .ToListAsync();

            List<TransactionArchive> archives = oldTransactions.Select(t => new TransactionArchive
            {
                TransactionId = t.Id,
                ProductId = t.ProductId,
                TransactionType = t.TransactionType,
                Quantity = t.Quantity,
                Date = t.Date,
                SourceWarehouseId = t.SourceWarehouseId,
                DestinationWarehouseId = t.DestinationWarehouseId,
                ArchivedDate = DateTime.UtcNow
            }).ToList();

            _context.TransactionArchives.AddRange(archives);
            _context.Transactions.RemoveRange(oldTransactions);
            await _context.SaveChangesAsync();
        }
    }

}
