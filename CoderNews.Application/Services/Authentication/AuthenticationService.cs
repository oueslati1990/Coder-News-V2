using CoderNews.Application.Common.Interfaces.Authentication;

namespace CoderNews.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public AuthenticationResult Login(string Email, string Password)
    {
        return new AuthenticationResult(Guid.NewGuid(),"firstName","lastName",Email,"token");
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // Check if user already exists

        // Create user

        // Create token for the user
        var userId = Guid.NewGuid();
        var token  = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);

        return new AuthenticationResult(userId,firstName,lastName,email,token);
    }
}
