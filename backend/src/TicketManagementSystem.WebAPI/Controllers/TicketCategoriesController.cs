using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.TicketCategories.CreateTicketCategory;
using TicketManagementSystem.Application.Features.TicketCategories.GetTicketCategories;

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
}