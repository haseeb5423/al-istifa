using Q_A_Backend.DTOs;
using Q_A_Backend.Models;

namespace Q_A_Backend.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<Guid> RegisterStep1Async(User user);
        public Task<bool> RegisterStep2Async(User user);

        public Task<User?> GetUserByIdAsync(Guid userId);

        public Task<User> LoginAsync(LoginDto dto);

        //Fetching Admin Role//
        public Task<User> GetUserByEmailAsync(string email);
        //Creating Admin Role//
        Task CreateAsync(User user);
    }
}
