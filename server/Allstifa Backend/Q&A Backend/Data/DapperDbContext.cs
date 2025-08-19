using Microsoft.Data.SqlClient;
using System.Data;

namespace Q_A_Backend.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

    }
}
