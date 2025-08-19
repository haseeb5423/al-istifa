using Microsoft.AspNetCore.Identity;
using Q_A_Backend.Models;
using Q_A_Backend.Repositories.Interfaces;
using Q_A_Backend.Services.Interfaces;

namespace Q_A_Backend.Seeder
{
    public class AdminSeeder : IAdminSeeder
{
    private readonly IAuthRepository _authRepo;
        private readonly IPasswordService _passwordService;


        public AdminSeeder(IAuthRepository authRepo, IPasswordService passwordService)
        {
            _authRepo = authRepo;

            _passwordService = passwordService;
        }

    public async Task SeedAdminAsync()
    {
        var existing = await _authRepo.GetUserByEmailAsync("haseebjoiya607@gmail.com");
        if (existing != null) return;

        var password = "Haseeb_007";
            var hashedPassword = _passwordService.HashPassword(password);

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "Super Admin",
            Email = "haseebjoiya607@gmail.com",
            PasswordHash = hashedPassword,
            Role = "Admin",
            IsVerified = true,
        };


        await _authRepo.CreateAsync(admin);
        Console.WriteLine("✅ Admin user seeded.");
    }
}

}