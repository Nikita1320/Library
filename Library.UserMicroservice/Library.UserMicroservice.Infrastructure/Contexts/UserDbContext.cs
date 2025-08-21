using Library.UserMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.UserMicroservice.Infrastructure.Contexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    }
}
