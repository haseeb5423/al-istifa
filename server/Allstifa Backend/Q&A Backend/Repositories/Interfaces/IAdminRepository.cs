using Q_A_Backend.DTOs;
using Q_A_Backend.Models;

namespace Q_A_Backend.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<UserForAdminDto>> GetUsersForAdminAsync(string? search, string? sortBy, string? sortDir,int pageNumber, int pageSize );
        Task<UserForAdminDto> GetUserByIdAsync(Guid userId);

    }
}