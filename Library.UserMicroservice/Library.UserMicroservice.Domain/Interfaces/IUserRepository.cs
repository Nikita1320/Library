using Library.UserMicroservice.Domain.Models;

namespace Library.UserMicroservice.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        void Update(User user);
        void Remove(User user);
        Task SaveChangesAsync();
        Task<List<User>> GetByIdAsync(List<Guid> guids);
        Task<User> GetByUserLoginAsync(string username);
    }
}
