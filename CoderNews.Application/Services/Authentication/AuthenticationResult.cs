using CoderNews.Domain.Entities;

namespace CoderNews.Application.Services.Authentication;

public record AuthenticationResult(
    User User,
    string Token
);
