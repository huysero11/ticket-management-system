using FluentValidation;

namespace TicketManagementSystem.Application.Features.TicketComments.GetTicketComments;

public sealed class GetTicketCommentsQueryValidator
    : AbstractValidator<GetTicketCommentsQuery>
{
    public GetTicketCommentsQueryValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}