using CoderNews.Application.Authentication.Common;
using CoderNews.Application.Authentication.Queries.Login;
using CoderNews.Authentication.Commands.Register;
using CoderNews.Contracts.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoderNews.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommand = new RegisterCommand(request.FirstName,
                                            request.LastName,
                                            request.Email,
                                            request.Password);

        var authResult = await _mediator.Send(registerCommand);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loginQuery = new LoginQuery(request.Email , request.Password);

        var authResult = await _mediator.Send(loginQuery);

        if (authResult.IsError && authResult.FirstError.Type == ErrorType.Validation)
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
