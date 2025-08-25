namespace Q_A_Backend.Repositories.Interfaces
{
    using Q_A_Backend.DTOs;
    using System;
    using System.Threading.Tasks;

    public interface IProfileRepository
    {
        Task<ProfileDto> GetProfileByIdAsync(Guid userId);

        Task<bool> UpdateProfileAsync(
            Guid userId, 
            ProfileUpdateDto profileUpdateDto);
    }
}