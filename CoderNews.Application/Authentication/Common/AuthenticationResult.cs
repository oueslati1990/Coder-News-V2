using CoderNews.Domain.Entities;

namespace CoderNews.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token
);
