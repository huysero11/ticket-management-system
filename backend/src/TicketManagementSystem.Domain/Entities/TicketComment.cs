using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Domain.Entities;

public sealed class TicketComment : AuditableEntity
{
    public Guid TicketId {get; private set;}
    public Ticket Ticket {get; private set;} = null!;

    public Guid UserId {get; private set;}
    public User User {get; private set;} = null!;

    public string Message {get; private set;} = null!;


    private TicketComment() {}

    public static TicketComment Create(Guid ticketId, Guid userId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Comment message cannot be empty.", nameof(message));
        }
        
        return new TicketComment
        {
            TicketId = ticketId,
            UserId = userId,
            Message = message,
        };
    }
}