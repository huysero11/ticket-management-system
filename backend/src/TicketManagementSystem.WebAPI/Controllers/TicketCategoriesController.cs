using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory;
using TicketManagementSystem.Application.Features.TicketCategories.DeleteTicketCategory;
using TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;
using TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategoryById;
using TicketManagementSystem.Application.Features.TicketCategories.UpdateTicketCategory;

namespace TicketManagementSystem.WebAPI.Controllers;

[ApiController]
[Route("api/ticket-categories")]
public sealed class TicketCategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public TicketCategoriesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sender.Send(command, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, new
            {
                status = "success",
                message = "Ticket category created successfully.",
                data = result
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                status = "failed",
                message = exception.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetTicketCategoriesQuery query, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket categories retrieved successfully.",
            data = result
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTicketCategoryByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Ticket category retrieved successfully.",
            data = result
        });

    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id, 
        [FromBody] UpdateTicketCategoryCommand command, 
        CancellationToken cancellationToken)
    {
        try 
        {
            var updateTicketCategoryCommand = new UpdateTicketCategoryCommand(id, command.Name, command.Description);
            await _sender.Send(updateTicketCategoryCommand, cancellationToken);
            return Ok(new
            {
                status = "success",
                message = "Ticket category updated successfully."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                status = "failed",
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                status = "failed",
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteTicketCategoryCommand(id);
            await _sender.Send(command, cancellationToken);
            
            return Ok(new
            {
                status = "success",
                message = "Ticket category deleted successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                status = "failed",
                message = ex.Message
            });
        }
    }
}