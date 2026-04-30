using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Tickets.CreateTicket;
using TicketManagementSystem.Application.Features.Tickets.GetTicketById;
using TicketManagementSystem.Application.Features.Tickets.GetTickets;

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetTicketByIdQuery(id);
        var ticket = await _sender.Send(query, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket retrieved successfully.",
            data = ticket
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetTicketsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query, cancellationToken);

        return Ok(new
        {
            status = "success",
            data = result
        });
    }
}