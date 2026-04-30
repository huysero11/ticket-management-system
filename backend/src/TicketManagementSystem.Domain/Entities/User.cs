using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Domain.Entities;

public class User : AuditableEntity
{
    public string Email {get; private set; } = default!;
    public string PasswordHash {get; private set; } = default!;
    public string FullName {get; private set; } = default!;

    public UserRole Role { get; private set; }

    private User() {}

    public User(string email, string passwordHash, string fullName, UserRole role)
    {
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
    }

    public void UpdateFullName(string fullName)
    {
        FullName = fullName;
        SetUpdated();
    }
}