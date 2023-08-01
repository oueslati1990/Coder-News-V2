using CoderNews.Domain.Entities;

namespace CoderNews.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
