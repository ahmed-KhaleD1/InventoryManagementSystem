using Azure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.CQRs.Transactions.Commands
{
    public class TransferStockCommand : IRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int SourceWarehouseId { get; set; }
        public int DestinationWarehouseId { get; set; }
    }
}
