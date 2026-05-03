using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Domain.Entities;

public sealed class Ticket : AuditableEntity
{
    public string Title {get; private set;} = null;
    public string Description {get; private set;} = null;

    public Guid CategoryId {get; private set;}
    public TicketCategory Category {get; private set;} = null;

    public TicketStatus Status {get; private set;}
    public TicketPriority Priority {get; private set;} 

    public Guid CreatedByUserId {get; private set;}
    public Guid? AssignedToUserId {get; private set;}

    public DateTime? ClosedAtUtc {get; private set;}

    private Ticket(){}

    public static Ticket Create(
        string title,
        string description,
        Guid categoryId,
        TicketPriority priority,
        Guid createdByUserId)
    {
        return new Ticket
        {
            Title = title,
            Description = description,
            CategoryId = categoryId,
            Priority = priority,
            Status = TicketStatus.Open,
            CreatedByUserId = createdByUserId
        };
    }

    public void Assign(Guid userId)
    {
        AssignedToUserId = userId;
        SetUpdated();
    }

    public void ChangeStatus(TicketStatus status)
    {
        if (Status == TicketStatus.Closed)
        {
            throw new InvalidOperationException("Closed ticket cannot be changed.");
        }

        if (Status == status)
        {
            return;
        }

        Status = status;

        if (status == TicketStatus.Closed)
        {
            ClosedAtUtc = DateTime.UtcNow;
        }

        SetUpdated();
    }

    public void Update(string title, string description, TicketPriority priority, Guid categoryId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        CategoryId = categoryId;
        SetUpdated();
    }
}