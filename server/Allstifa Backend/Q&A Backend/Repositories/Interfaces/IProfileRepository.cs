namespace Q_A_Backend.Repositories.Interfaces
{
    using Q_A_Backend.DTOs;
    using Q_A_Backend.Models;

    using System;
    using System.Threading.Tasks;

    public interface IProfileRepository
    {
        Task<ProfileDto> GetProfileByIdAsync(Guid userId);

        Task<User> UpdateProfileAsync(
            Guid userId, 
            ProfileUpdateDto profileUpdateDto);
    }
}