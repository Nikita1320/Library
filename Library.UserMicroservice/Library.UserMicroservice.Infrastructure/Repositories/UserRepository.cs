using Library.UserMicroservice.Domain.Interfaces;
using Library.UserMicroservice.Domain.Models;
using Library.UserMicroservice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Library.UserMicroservice.Infrastructure.Repositories
{
    public class UserRepository(UserDbContext userDbContext) : IUserRepository
    {
        public async Task<List<User>> GetByIdAsync(List<Guid> guids)
        {
            return await userDbContext.Users.Where(p => guids.Contains(p.Id)).ToListAsync();
        }
        public async Task Add(User user)
        {
            await userDbContext.Users.AddAsync(user);
        }

        public async Task AddAsync(User entity)
        {
            await userDbContext.Users.AddAsync(entity);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await userDbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await userDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUserLoginAsync(string login)
        {
            var user = await userDbContext.Users
                .FirstOrDefaultAsync(c => c.Login == login);

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public void Remove(User entity)
        {
            userDbContext.Users.Remove(entity);
        }

        public void Update(User entity)
        {
            userDbContext.Users.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await userDbContext.SaveChangesAsync();
        }
    }
}
