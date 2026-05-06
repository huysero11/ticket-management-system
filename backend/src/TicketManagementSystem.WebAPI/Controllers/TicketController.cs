using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.TicketComments.AddTicketComment;
using TicketManagementSystem.Application.Features.TicketComments.GetTicketComments;
using TicketManagementSystem.Application.Features.Tickets.AssignTicket;
using TicketManagementSystem.Application.Features.Tickets.ChangeTicketStatus;
using TicketManagementSystem.Application.Features.Tickets.CreateTicket;
using TicketManagementSystem.Application.Features.Tickets.GetTicketById;
using TicketManagementSystem.Application.Features.Tickets.GetTickets;
using TicketManagementSystem.Application.Features.Tickets.UpdateTicket;
using TicketManagementSystem.WebAPI.Contracts.TicketComments;
using TicketManagementSystem.WebAPI.Contracts.Tickets;

namespace TicketManagementSystem.WebAPI.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
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

    [HttpPatch("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignTicketRequest request, CancellationToken cancellationToken)
    {
        var command = new AssignTicketCommand(id, request.AssignedToUserId);
        await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket assigned successfully."
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTicketCommand(id, request.Title, request.Description, request.Priority, request.CategoryId);
        await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket updated successfully."
        });
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeTicketStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeTicketStatusCommand(id, request.Status);
        await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket status changed successfully."
        });
    }

    [HttpPost("{ticketId:guid}/comments")]
    public async Task<IActionResult> AddComment(
        Guid ticketId,
        [FromBody] AddTicketCommentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddTicketCommentCommand(
            ticketId,
            request.Message);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Comment added successfully.",
            data = result
        });
    }

    [HttpGet("{ticketId:guid}/comments")]
    public async Task<IActionResult> GetComments(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var query = new GetTicketCommentsQuery(ticketId);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket comments retrieved successfully.",
            data = result
        });
    }


}