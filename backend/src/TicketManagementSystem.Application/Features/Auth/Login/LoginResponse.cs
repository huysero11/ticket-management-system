namespace TicketManagementSystem.Application.Features.Auth.Login;


public sealed record LoginResponse(
    string AccessToken,
    LoginUserResponse User
);

public sealed record LoginUserResponse (
    Guid Id,
    string Email,
    string FullName
);