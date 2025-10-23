using CoderNews.Application.Authentication.Common;
using CoderNews.Application.Authentication.Queries.Login;
using CoderNews.Authentication.Commands.Register;
using CoderNews.Contracts.Authentication;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoderNews.Api.Controllers;

/// <summary>
/// Authentication controller for user registration and login
/// </summary>
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

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="request">User registration details</param>
    /// <returns>Authentication response with JWT token</returns>
    /// <response code="200">User successfully registered</response>
    /// <response code="400">Invalid registration data</response>
    /// <response code="409">Email already exists</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(request);

        var authResult = await _mediator.Send(registerCommand);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
        );
    }


    /// <summary>
    /// Authenticate an existing user
    /// </summary>
    /// <param name="request">User login credentials</param>
    /// <returns>Authentication response with JWT token</returns>
    /// <response code="200">User successfully authenticated</response>
    /// <response code="400">Invalid login data</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
