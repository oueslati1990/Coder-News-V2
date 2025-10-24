using CoderNews.Application.Common.Interfaces.Persistence;
using CoderNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoderNews.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly CoderNewsDbContext _context;

    public UserRepository(CoderNewsDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserByEmail(string email)
    {
        return _context.Users.SingleOrDefault(u => u.Email == email);
    }
}
