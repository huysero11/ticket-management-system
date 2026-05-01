using System.Security.Claims;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.WebAPI.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => 
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException("User id claim not found");
            }

            return Guid.Parse(userId);
        }
    }

        public UserRole Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
                if (string.IsNullOrWhiteSpace(role))
                {
                    throw new UnauthorizedAccessException("User role claim not found");
                }

                return Enum.Parse<UserRole>(role);
            }
        }
    
}