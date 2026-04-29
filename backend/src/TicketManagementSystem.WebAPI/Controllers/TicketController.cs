using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Tickets.CreateTicket;

namespace TicketManagementSystem.WebAPI.Controllers;

[ApiController]
[Route("api/tickets")]
public sealed class TicketsController : ControllerBase
{
    private readonly ISender _sender;

    public TicketsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket created successfully.",
            data = id
        });
    }
}