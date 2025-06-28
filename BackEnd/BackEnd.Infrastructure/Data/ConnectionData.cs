using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Data
{
    public class ConnectionData
    {
        private readonly string _connectionString;

        public ConnectionData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
