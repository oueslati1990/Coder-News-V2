using CoderNews.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace CoderNews.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;