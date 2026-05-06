using FluentValidation;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Tickets.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandValidator
    : AbstractValidator<ChangeTicketStatusCommand>
{
    public ChangeTicketStatusCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum()
            .Must(status => status != TicketStatus.Open)
            .WithMessage("Cannot manually change ticket status back to Open.");
    }
}