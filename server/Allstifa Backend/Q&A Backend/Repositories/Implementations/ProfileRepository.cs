namespace Q_A_Backend.Repositories.Implementations
{
    using Dapper;
    using Q_A_Backend.Data;
    using Q_A_Backend.DTOs;
    using Q_A_Backend.Models;

    using Q_A_Backend.Repositories.Interfaces;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class ProfileRepository : IProfileRepository
    {
        private readonly DapperDbContext _dbContext;
        public ProfileRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProfileDto> GetProfileByIdAsync(Guid userId)
        {


            using var connection = _dbContext.CreateConnection();
            var sql = @"SELECT Id, Name, Email, Bio, Role, MaslakId, ProfileImagePath, CreatedAt, ProofFilePath, SocialMediaLink
                        FROM Users WHERE Id = @UserId";
            var result = await connection.QuerySingleOrDefaultAsync<ProfileDto>(sql, new { UserId = userId }).ConfigureAwait(false);
            if (result == null)
            {
                return null;
            }
            return result;

        }
        public async Task<User> UpdateProfileAsync(
    Guid userId,
    ProfileUpdateDto profileUpdateDto)
        {
            using var connection = _dbContext.CreateConnection();

            // 1. Check if a new email is provided and if it's already taken by ANOTHER user
            if (!string.IsNullOrEmpty(profileUpdateDto.Email))
            {
                var emailCheckSql = "SELECT Id FROM Users WHERE Email = @Email AND Id != @UserId";
                var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
                    emailCheckSql,
                    new { Email = profileUpdateDto.Email, UserId = userId }
                ).ConfigureAwait(false);

                // 2. If another user with that email exists, throw a specific error
                if (existingUser != null)
                {
                    throw new Exception("Email is already in use.");
                }
            }

            // 3. If the check passes, proceed with your original update logic
            var updateSql = @"UPDATE Users SET 
                        Name = COALESCE(@Name, Name), 
                        Email = COALESCE(@Email, Email), 
                        Bio = COALESCE(@Bio, Bio), 
                        ProfileImagePath = COALESCE(@ProfileImagePath, ProfileImagePath), 
                        MaslakId = COALESCE(@MaslakId, MaslakId), 
                        IsVerified = COALESCE(@IsVerified, IsVerified), 
                        ProofFilePath = COALESCE(@ProofFilePath, ProofFilePath),
                        SocialMediaLink = COALESCE(@SocialMediaLink, SocialMediaLink)
                      WHERE Id = @UserId";

            var parameters = new
            {
                Name = profileUpdateDto.Name,
                Email = profileUpdateDto.Email,
                Bio = profileUpdateDto.Bio,
                ProfileImagePath = profileUpdateDto.ProfileImagePath,
                MaslakId = profileUpdateDto.MaslakId,
                IsVerified = profileUpdateDto.IsVerified,
                ProofFilePath = profileUpdateDto.ProofFilePath,
                SocialMediaLink = profileUpdateDto.SocialMediaLink,
                UserId = userId
            };

            var affectedRows = await connection.ExecuteAsync(updateSql, parameters).ConfigureAwait(false);

            if (affectedRows == 0)
            {
                throw new InvalidOperationException($"User with ID {userId} not found or update failed.");
            }

            var selectSql = "SELECT * FROM Users WHERE Id = @UserId";
            var updatedUser = await connection.QuerySingleOrDefaultAsync<User>(selectSql, new { UserId = userId }).ConfigureAwait(false);
            return updatedUser;
        }
    }
}