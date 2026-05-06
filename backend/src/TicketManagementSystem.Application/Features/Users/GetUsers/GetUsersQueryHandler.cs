using MediatR;
using TicketManagementSystem.Application.Abstractions.Repositories;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, IReadOnlyList<GetUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUsersQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<GetUserResponse>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.Role != UserRole.Admin)
        {
            throw new UnauthorizedAccessException("Only admin can view users.");
        }

        var users = await _userRepository.GetUsersAsync(
            query.Role,
            cancellationToken);

        return users
            .Select(user => new GetUserResponse(
                user.Id,
                user.Email,
                user.FullName,
                user.Role))
            .ToList();
    }
}