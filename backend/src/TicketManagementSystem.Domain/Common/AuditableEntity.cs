namespace TicketManagementSystem.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; protected set; }

    public void SetUpdated()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}