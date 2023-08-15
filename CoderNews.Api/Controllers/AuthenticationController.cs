using CoderNews.Application.Services.Authentication;
using CoderNews.Contracts.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace CoderNews.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var authResult = _authenticationService.Register(request.FirstName,
                                                 request.LastName,
                                                 request.Email,
                                                 request.Password);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(request.Email, request.Password);

        if(authResult.IsError && authResult.FirstError.Type == ErrorType.Validation)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized,
                           title: authResult.FirstError.Description);
        }
        
        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
                        
    }

    private AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(authResult.User.Id,
                                                  authResult.User.FirstName,
                                                  authResult.User.LastName,
                                                  authResult.User.Email,
                                                  authResult.Token);
    }
}
