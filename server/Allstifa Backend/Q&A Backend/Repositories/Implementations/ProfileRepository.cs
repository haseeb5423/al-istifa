namespace Q_A_Backend.Repositories.Implementations
{
    using Dapper;
    using Q_A_Backend.Data;
    using Q_A_Backend.DTOs;
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
        public async Task<bool> UpdateProfileAsync(
            Guid userId, 
            ProfileUpdateDto profileUpdateDto)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"UPDATE Users SET 
                        Name = COALESCE(@Name, Name), 
                        Email = COALESCE(@Email, Email), 
                        Bio = COALESCE(@Bio, Bio), 
                        ProfileImagePath = COALESCE(@ProfileImagePath, ProfileImagePath), 
                        MaslakId = COALESCE(@MaslakId, MaslakId), 
                        IsVerified = COALESCE(@IsVerified, IsVerified), 
                        ProofFilePath = COALESCE(@ProofFilePath, ProofFilePath),
                        SocialMediaLink = COALESCE(@SocialMediaLink, SocialMediaLink)
                        WHERE Id = @UserId";

            var result = await connection.ExecuteAsync(sql, new
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
            }).ConfigureAwait(false);
            if (result < 0)
            {
                Debug.WriteLine("UpdateProfileAsync failed for userId: " + userId);
            }
            return result > 0;
        }
    }
}