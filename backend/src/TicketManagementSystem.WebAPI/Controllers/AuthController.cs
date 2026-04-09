using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Auth.Register;

namespace TicketManagementSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sender.Send(command, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, new
            {
                status = "success",
                message = "User registered successfully.",
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
}