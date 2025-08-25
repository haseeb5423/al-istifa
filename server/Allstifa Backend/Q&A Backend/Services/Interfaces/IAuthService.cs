
using Q_A_Backend.DTOs;
using Q_A_Backend.Models;

namespace Q_A_Backend.Services.Interfaces
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public User? User { get; set; } 
    }

    public interface IAuthService
    {
        public Task<Guid?> RegisterStep1Async(RegisterStep1Dto registerStep1Dto);
        public Task<bool> RegisterStep2Async(RegisterStep2Dto registerStep2Dto);
        public Task<LoginResult> LoginAsync(LoginDto dto);
    }
}
