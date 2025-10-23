using CoderNews.Application.Authentication.Common;
using CoderNews.Application.Common.Interfaces.Authentication;
using CoderNews.Application.Common.Interfaces.Persistence;
using CoderNews.Authentication.Commands.Register;
using CoderNews.Domain.Entities;
using CoderNews.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CoderNews.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // 1. Check if user already exists
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Create user and persist it to the database
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = _passwordHasher.HashPassword(command.Password)
        };
        _userRepository.Add(user);

        // 3. Create token for the user

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
