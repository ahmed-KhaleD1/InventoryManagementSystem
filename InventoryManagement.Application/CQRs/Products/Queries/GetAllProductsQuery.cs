using InventoryManagement.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.CQRs.Products.Queries
{
    public class GetAllProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
