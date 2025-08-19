using Microsoft.AspNetCore.Identity;
using Q_A_Backend.Services.Interfaces;

namespace Q_A_Backend.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordService(IPasswordHasher<object> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
