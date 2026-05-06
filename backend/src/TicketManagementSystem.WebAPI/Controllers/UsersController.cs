using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Users.GetUsers;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] UserRole? role,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(role);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Users retrieved successfully.",
            data = result
        });
    }
}