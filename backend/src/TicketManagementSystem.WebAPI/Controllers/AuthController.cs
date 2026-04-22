using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Auth.GetMe;
using TicketManagementSystem.Application.Features.Auth.Login;
using TicketManagementSystem.Application.Features.Auth.Refresh;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sender.Send(command, cancellationToken);

            return Ok(new
            {
                status = "success",
                message = "User logged in successfully.",
                data = result
            });
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new
            {
                status = "failed",
                message = exception.Message
            });
        }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var fullName = User.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrWhiteSpace(userIdValue) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(fullName))
        {
            return Unauthorized(new
            {
                status = "failed",
                message = "Invalid token."
            });
        }

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new
            {
                status = "failed",
                message = "Invalid token."
            });
        }

        var response = new GetMeResponse(userId, email, fullName);
        return Ok(new
        {
            status = "success",
            message = "User information retrieved successfully.",
            data = response
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sender.Send(command, cancellationToken);

            return Ok(new
            {
                status = "success",
                message = "Token refreshed successfully.",
                data = result
            });
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new
            {
                status = "failed",
                message = exception.Message
            });
        }
    }
}