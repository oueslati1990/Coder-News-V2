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

    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
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
        if (user.Password != query.Password)
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        // 3. Generate token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
