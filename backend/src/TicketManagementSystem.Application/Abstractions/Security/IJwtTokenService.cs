namespace TicketManagementSystem.Application.Abstractions.Security;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, string fullName);
}