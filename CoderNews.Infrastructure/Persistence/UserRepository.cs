using CoderNews.Application.Common.Interfaces.Persistence;
using CoderNews.Domain.Entities;

namespace CoderNews.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> users = new ();
    public void Add(User user)
    {
        users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return users.SingleOrDefault(u => u.Email == email);
    }
}
