using Q_A_Backend.Models;

namespace Q_A_Backend.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
