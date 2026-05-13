using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Features.Dashboard.GetDashboardSummary;

namespace TicketManagementSystem.WebAPI.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public sealed class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetDashboardSummaryQuery(),
            cancellationToken);

        return Ok(new
        {
            status = "success",
            message = "Dashboard summary retrieved successfully.",
            data = result
        });
    }
}