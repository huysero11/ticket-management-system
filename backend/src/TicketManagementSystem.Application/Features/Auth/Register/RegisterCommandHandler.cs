using MediatR;
using TicketManagementSystem.Application.Abstractions.Security;
using TicketManagementSystem.Domain.Entities;
using TicketManagementSystem.Application.Abstractions.Repositoties;

namespace TicketManagementSystem.Application.Features.Auth.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var fullName = request.FullName.Trim();

        var isEmailExists = await _userRepository.ExistsByEmailAsync(email, cancellationToken);
        if (isEmailExists)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User(email, passwordHash, fullName);

        await _userRepository.AddAsync(user, cancellationToken);

        return new RegisterResponse(user.Id, user.Email, user.FullName);
    }
}