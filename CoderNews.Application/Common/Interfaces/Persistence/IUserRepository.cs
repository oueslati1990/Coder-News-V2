using CoderNews.Domain.Entities;

namespace CoderNews.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
}