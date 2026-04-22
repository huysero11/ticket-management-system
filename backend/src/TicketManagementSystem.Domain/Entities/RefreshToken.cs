using TicketManagementSystem.Domain.Common;
namespace TicketManagementSystem.Domain.Entities;

public sealed class RefreshToken : AuditableEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = default!;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; } 

    public User User { get; private set; } = default!;

    private RefreshToken() {}

    public RefreshToken(Guid userId, string token, DateTime expiresAtUtc)
    {
        UserId = userId;
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
    }

    public void Revoke()
    {
        RevokedAtUtc = DateTime.UtcNow;
        SetUpdated();
    }
}