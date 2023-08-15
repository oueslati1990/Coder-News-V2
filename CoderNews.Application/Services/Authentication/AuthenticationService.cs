using CoderNews.Application.Common.Interfaces.Authentication;
using CoderNews.Application.Common.Interfaces.Persistence;
using CoderNews.Domain.Entities;
using ErrorOr;
using CoderNews.Domain.Common.Errors;

namespace CoderNews.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);
        // 1. Validate user exists
        if (user == null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validate password is correct
        if (user.Password != password)
        {
            return new[] {Errors.Authentication.InvalidCredentials};
        }

        // 3. Generate token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // 1. Check if user already exists
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Create user and persist it to the database
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        _userRepository.Add(user);

        // 3. Create token for the user

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
