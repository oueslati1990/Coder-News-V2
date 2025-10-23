using CoderNews.Application.Authentication.Common;
using CoderNews.Application.Common.Interfaces.Authentication;
using CoderNews.Application.Common.Interfaces.Persistence;
using CoderNews.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CoderNews.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserByEmail(query.Email);
        // 1. Validate user exists
        if (user == null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validate password is correct
        if (!_passwordHasher.VerifyPassword(query.Password, user.Password))
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        // 3. Generate token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
