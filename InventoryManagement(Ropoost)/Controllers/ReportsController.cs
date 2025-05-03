using InventoryManagement.Application.CQRs.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace InventoryManagement_Ropoost_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockReport([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var report = await _mediator.Send(new LowStockReportQuery { Page = page, PageSize = pageSize });
            return Ok(report);
        }

        [HttpGet("transaction-history")]
        public async Task<IActionResult> GetTransactionHistory(
            [FromQuery] int? productId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new TransactionHistoryQuery
            {
                ProductId = productId,
                StartDate = startDate,
                EndDate = endDate,
                Page = page,
                PageSize = pageSize
            };
            var history = await _mediator.Send(query);
            return Ok(history);
        }
    }
}
