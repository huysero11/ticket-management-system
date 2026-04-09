using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Domain.Entities;

public class User : AuditableEntity
{
    public string Email {get; private set; } = default!;
    public string PasswordHash {get; private set; } = default!;
    public string FullName {get; private set; } = default!;

    private User() {}

    public User(string email, string passwordHash, string fullName)
    {
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
    }

    public void UpdateFullName(string fullName)
    {
        FullName = fullName;
        SetUpdated();
    }
}