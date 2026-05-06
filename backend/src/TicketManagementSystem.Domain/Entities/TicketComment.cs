using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Domain.Entities;

public sealed class TicketComment : AuditableEntity
{
    public Guid TicketId { get; private set; }
    public Ticket Ticket { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string Message { get; private set; } = null!;

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }
    public Guid? DeletedByUserId { get; private set; }

    private TicketComment()
    {
    }

    public static TicketComment Create(
        Guid ticketId,
        Guid userId,
        string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Comment message cannot be empty.", nameof(message));
        }

        return new TicketComment
        {
            TicketId = ticketId,
            UserId = userId,
            Message = message.Trim(),
            IsDeleted = false
        };
    }

    public void UpdateMessage(string message)
    {
        if (IsDeleted)
        {
            throw new InvalidOperationException("Deleted comment cannot be updated.");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Comment message cannot be empty.", nameof(message));
        }

        Message = message.Trim();
        SetUpdated();
    }

    public void SoftDelete(Guid deletedByUserId)
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
        DeletedByUserId = deletedByUserId;
        SetUpdated();
    }
}