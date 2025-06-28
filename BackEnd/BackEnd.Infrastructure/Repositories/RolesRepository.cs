using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly ConnectionData _connectionData;

        public RolesRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Roles>> ReadRoles()
        {
            var roles = new List<Roles>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_ROLE, ROLE_NAME, STATUS, CREATION_DATE FROM ROLES ORDER BY PK_ROLE", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                roles.Add(new Roles
                                {
                                    PK_ROLE = Convert.ToInt32(reader["PK_ROLE"]),
                                    ROLE_NAME = reader["ROLE_NAME"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading roles.", ex);
            }
        }

        public async Task<IEnumerable<Roles>> SelectRoles()
        {
            var roles = new List<Roles>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_ROLE, ROLE_NAME FROM ROLES WHERE STATUS <> 'INACTIVO' ORDER BY ROLE_NAME", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                roles.Add(new Roles
                                {
                                    PK_ROLE = Convert.ToInt32(reader["PK_ROLE"]),
                                    ROLE_NAME = reader["ROLE_NAME"].ToString(),
                                });
                            }
                        }
                    }
                }
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while selecting roles.", ex);
            }
        }
    }
}
