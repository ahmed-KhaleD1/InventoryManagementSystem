using InventoryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Entities
{
    
    public class Transaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public int? SourceWarehouseId { get; set; }
        public Warehouse? SourceWarehouse { get; set; }
        public int? DestinationWarehouseId { get; set; }
        public Warehouse? DestinationWarehouse { get; set; }
    }
}
