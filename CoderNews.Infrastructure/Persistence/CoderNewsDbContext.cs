using CoderNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoderNews.Infrastructure.Persistence;

public class CoderNewsDbContext : DbContext
{
    public CoderNewsDbContext(DbContextOptions<CoderNewsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoderNewsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
