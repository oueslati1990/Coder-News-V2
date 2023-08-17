using CoderNews.Application.Authentication.Common;
using CoderNews.Application.Authentication.Queries.Login;
using CoderNews.Authentication.Commands.Register;
using CoderNews.Contracts.Authentication;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoderNews.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(request);

        var authResult = await _mediator.Send(registerCommand);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
        );
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loginQuery = _mapper.Map<LoginQuery>(request);

        var authResult = await _mediator.Send(loginQuery);

        if (authResult.IsError && authResult.FirstError.Type == ErrorType.Validation)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized,
                           title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
        );

    }
}
