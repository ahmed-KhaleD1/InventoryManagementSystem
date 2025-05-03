using InventoryManagement.Application.CQRs.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement_Ropoost_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddStock([FromBody] AddStockCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveStock([FromBody] RemoveStockCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferStock([FromBody] TransferStockCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
