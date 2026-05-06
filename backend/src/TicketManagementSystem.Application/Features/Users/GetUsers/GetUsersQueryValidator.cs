using FluentValidation;

namespace TicketManagementSystem.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Role)
            .IsInEnum()
            .When(x => x.Role.HasValue);
    }
}