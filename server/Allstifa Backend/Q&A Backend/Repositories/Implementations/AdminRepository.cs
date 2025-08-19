using Dapper;
using Q_A_Backend.Data;
using Q_A_Backend.DTOs;
using Q_A_Backend.Models;
using System.Linq;
using Q_A_Backend.Repositories.Interfaces;
using AutoMapper;
using System.Text;

namespace Q_A_Backend.Repositories.Implementations
{
    public class AdminRepository(DapperDbContext dbContext, IMapper mapper) : IAdminRepository
    {
        private readonly DapperDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;


        public async Task<IEnumerable<UserForAdminDto>> GetUsersForAdminAsync(
    string? search, string? sortBy, string? sortDir, int pageNumber, int pageSize)
        {
            using var connection = _dbContext.CreateConnection();

            var sql = new StringBuilder("SELECT * FROM Users WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search))
            {
                sql.Append(" AND (Name LIKE @Search OR Email LIKE @Search)");
            }

            var allowedSortColumns = new[] { "Name", "Email", "CreatedAt", "Role" };
            if (!string.IsNullOrEmpty(sortBy) && allowedSortColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            {
                var direction = (sortDir?.ToLower() == "desc") ? "DESC" : "ASC";
                sql.Append($" ORDER BY {sortBy} {direction}");
            }
            else
            {
                sql.Append(" ORDER BY CreatedAt DESC"); // default sort
            }

            // 🟢 Pagination
            int offset = (pageNumber - 1) * pageSize;
            sql.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            var users = await connection.QueryAsync<User>(
                sql.ToString(),
                new { Search = $"%{search}%", Offset = offset, PageSize = pageSize });

            return _mapper.Map<IEnumerable<UserForAdminDto>>(users);
        }


        public async Task<UserForAdminDto> GetUserByIdAsync(Guid userId)
        {
            using var connection = _dbContext.CreateConnection();

            var sql = @"SELECT Id, Name, Email, Bio, Role, MaslakId, ProfileImagePath, CreatedAt, ProofFilePath, SocialMediaLink 
                        FROM Users 
                        WHERE Id = @UserId";

            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });

            var result = _mapper.Map<UserForAdminDto>(user);
            if (result == null)
            {
                return null;
            }
            return result;
        }

    }
}