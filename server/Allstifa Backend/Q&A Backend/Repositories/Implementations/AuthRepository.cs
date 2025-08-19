using Dapper;
using Q_A_Backend.Data;
using Q_A_Backend.DTOs;
using Q_A_Backend.Models;
using Q_A_Backend.Repositories.Interfaces;
using Q_A_Backend.Services.Interfaces;
using System.Diagnostics;

namespace Q_A_Backend.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IPasswordService _passwordService;
        private readonly DapperDbContext _dbContext;

        public AuthRepository(DapperDbContext dbContext, IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Registers a new user (step 1) and returns the new user's ID.
        /// </summary>
        public async Task<Guid> RegisterStep1Async(User user)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var sql = @"INSERT INTO Users (Name, Email, PasswordHash, Role, MaslakId)
OUTPUT INSERTED.Id
VALUES (@Name, @Email, @PasswordHash, @Role, @MaslakId)";
                var parameters = new
                {
                    user.Name,
                    user.Email,
                    user.PasswordHash,
                    user.Role,
                    user.MaslakId
                };
                var userId = await connection.ExecuteScalarAsync<Guid>(sql, parameters).ConfigureAwait(false);
                return userId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RegisterStep1Async: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Updates user profile details (step 2).
        /// </summary>
        public async Task<bool> RegisterStep2Async(User user)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var sql = @"UPDATE Users SET Bio = @Bio, ProfileImagePath = @ProfileImagePath, ProofFilePath = @ProofFilePath, SocialMediaLink = @SocialMediaLink WHERE Id = @Id";
                var parameters = new
                {
                    user.Bio,
                    user.ProfileImagePath,
                    user.ProofFilePath,
                    user.SocialMediaLink,
                    user.Id
                };
                var result = await connection.ExecuteAsync(sql, parameters, commandTimeout: 30).ConfigureAwait(false);
                return result > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RegisterStep2Async: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var sql = @"SELECT * FROM Users WHERE Id = @Id";
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = userId }, commandTimeout: 60).ConfigureAwait(false);
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetUserByIdAsync: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Authenticates a user by email and password.
        /// </summary>
        public async Task<User> LoginAsync(LoginDto dto)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var sql = @"SELECT * FROM Users WHERE Email = @Email";
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = dto.Email }, commandTimeout: 30).ConfigureAwait(false);

                if (user == null)
                {
                    return null;
                }

               
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoginAsync: {ex}");
                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var sql = @"SELECT * FROM Users WHERE Email = @Email";
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email }, commandTimeout: 30).ConfigureAwait(false);
                if (user == null)
                {
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user by email", ex);
            }
        }


        public async Task CreateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"
            INSERT INTO Users (Id, Name, Email, PasswordHash, Role, IsVerified)
            VALUES (@Id, @Name, @Email, @PasswordHash, @Role, @IsVerified)";

            var result = await connection.ExecuteAsync(sql, user);
            if (result <= 0)
            {
                Debug.WriteLine("CreateAsync failed for admin: " + user.Email);
                throw new Exception("Failed to create admin");
            }
            else
            {
                Debug.WriteLine("User created successfully: " + user.Email);
            }
        }
    }
}