using FluentValidation;

namespace TicketManagementSystem.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandValidator
    : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.AssignedToUserId)
            .NotEmpty();
    }
}