using Microsoft.AspNetCore.Identity;
using TicketManagementSystem.Application.Abstractions.Security;

namespace TicketManagementSystem.Infrastructure.Security;

public sealed class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(new object(), password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(new object(), passwordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}