using Library.Contracts;
using Library.UserMicroservice.Domain.Interfaces;
using Library.UserMicroservice.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Library.UserMicroservice.Application.Services
{
    public class UserService(IUserRepository userRepository, JwtService jwtService, PasswordHasher passwordHasher, IPublishEndpoint publishEndpoint, ILogger<UserService> logger)
    {
        public async Task<User> Register(string login, string email, string userName, string password)
        {
            var existUser = await userRepository.GetByUserLoginAsync(userName);
            if (existUser == null)
            {
                var passHash = passwordHasher.Generate(password);

                User user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = userName,
                    Login = login,
                    Email = email,
                    PasswordHash = passHash
                };

                await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();
                logger.LogInformation($"User created with login: {login}");
                await publishEndpoint.Publish<UserCreated>(new
                {
                    UserId = Guid.NewGuid(),
                    Name = userName,
                    Email = email,
                    CreateTime = DateTime.Now,
                });
                return user;
            }
            else
            {
                logger.LogInformation($"User not created. User with login {login} already exist");
                throw new ArgumentException("User with that name already exists");
            }
        }
        public async Task<string> Login(string login, string password)
        {

            User user = await userRepository.GetByUserLoginAsync(login);
            if (user != null)
            {
                var result = passwordHasher.Verify(password, user.PasswordHash);
                if (result)
                {
                    return jwtService.GenerateToken(user);
                }
                else
                {
                    throw new ArgumentException("Verify password error");
                }
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await userRepository.GetByIdAsync(id);
        }
    }
}
