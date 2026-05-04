using FluentValidation;

namespace TicketManagementSystem.Application.Features.Tickets.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandValidator
    : AbstractValidator<ChangeTicketStatusCommand>
{
    public ChangeTicketStatusCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}