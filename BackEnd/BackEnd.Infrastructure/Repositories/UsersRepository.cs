using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ConnectionData _connectionData;

        public UsersRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Users>> ReadUsers()
        {
            var users = new List<Users>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT USERS.PK_USER, USERS.USER_NAME, USERS.PASSWORD_HASH, USERS.NAMES, USERS.LAST_NAMES, USERS.IDENTIFICATION, USERS.PHONE, USERS.STATUS, USERS.PK_ROLE, USERS.PK_STORE, USERS.CREATION_DATE, ROLES.ROLE_NAME, STORES.STORE_NAME
                                                                 FROM  USERS INNER JOIN ROLES ON USERS.PK_ROLE = ROLES.PK_ROLE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE ORDER BY PK_USER DESC", connection))
                    {

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new Users
                                {
                                    PK_USER = Convert.ToInt32(reader["PK_USER"]),
                                    USER_NAME = reader["USER_NAME"].ToString(),
                                    PASSWORD_HASH = (byte[])reader["PASSWORD_HASH"],
                                    NAMES = reader["NAMES"].ToString(),
                                    LAST_NAMES = reader["LAST_NAMES"].ToString(),
                                    IDENTIFICATION = reader["IDENTIFICATION"].ToString(),
                                    PHONE = reader["PHONE"] != DBNull.Value ? (int?)Convert.ToInt32(reader["PHONE"]) : null,
                                    STATUS = reader["STATUS"].ToString(),
                                    PK_ROLE = Convert.ToInt32(reader["PK_ROLE"]),
                                    PK_STORE = Convert.ToInt32(reader["PK_STORE"]),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),

                                    roles = new Roles
                                    {
                                        ROLE_NAME = reader["ROLE_NAME"].ToString()
                                    },
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading users.", ex);
            }

            return users;
        }

        public async Task<IEnumerable<Users>> SearchUser(string text)
        {
            var users = new List<Users>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT USERS.PK_USER, USERS.USER_NAME, USERS.PASSWORD_HASH, USERS.NAMES, USERS.LAST_NAMES, USERS.IDENTIFICATION, USERS.PHONE, USERS.STATUS, USERS.PK_ROLE, USERS.PK_STORE, USERS.CREATION_DATE, ROLES.ROLE_NAME, STORES.STORE_NAME
                                                                 FROM  USERS INNER JOIN ROLES ON USERS.PK_ROLE = ROLES.PK_ROLE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE 
																 WHERE (USERS.NAMES LIKE '%' + @text + '%' OR  ROLES.ROLE_NAME LIKE '%' + @text + '%' OR STORES.STORE_NAME LIKE '%' + @text + '%') ORDER BY PK_USER DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new Users
                                {
                                    PK_USER = Convert.ToInt32(reader["PK_USER"]),
                                    USER_NAME = reader["USER_NAME"].ToString(),
                                    PASSWORD_HASH = (byte[])reader["PASSWORD_HASH"],
                                    NAMES = reader["NAMES"].ToString(),
                                    LAST_NAMES = reader["LAST_NAMES"].ToString(),
                                    IDENTIFICATION = reader["IDENTIFICATION"].ToString(),
                                    PHONE = reader["PHONE"] != DBNull.Value ? (int?)Convert.ToInt32(reader["PHONE"]) : null,
                                    STATUS = reader["STATUS"].ToString(),
                                    PK_ROLE = Convert.ToInt32(reader["PK_ROLE"]),
                                    PK_STORE = Convert.ToInt32(reader["PK_STORE"]),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),

                                    roles = new Roles
                                    {
                                        ROLE_NAME = reader["ROLE_NAME"].ToString()
                                    },
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching user.", ex);
            }

            return users;
        }

        public async Task<string> CreateUser(Users user, string password)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

                GeneratePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT COUNT(*) FROM USERS WHERE USER_NAME = @user", connection))
                    {
                        command.Parameters.AddWithValue("@user", user.USER_NAME);

                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            return "El usuario ya existe.";
                        }
                        else 
                        {

                            command.Parameters.Add("@USER_NAME", SqlDbType.NVarChar, 25).Value = user.USER_NAME;
                            command.Parameters.Add("@PASSWORD_HASH", SqlDbType.VarBinary, -1).Value = passwordHash;
                            command.Parameters.Add("@PASSWORD_SALT", SqlDbType.VarBinary, -1).Value = passwordSalt;
                            command.Parameters.Add("@NAMES", SqlDbType.NVarChar, 30).Value = user.NAMES;
                            command.Parameters.Add("@LAST_NAMES", SqlDbType.NVarChar, 30).Value = user.LAST_NAMES;
                            command.Parameters.Add("@IDENTIFICATION", SqlDbType.VarChar, 10).Value = user.IDENTIFICATION ?? (object)DBNull.Value;
                            command.Parameters.Add("@PHONE", SqlDbType.Int).Value = user.PHONE ?? (object)DBNull.Value;
                            command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                            command.Parameters.Add("@PK_ROLE", SqlDbType.Int).Value = user.PK_ROLE;
                            command.Parameters.Add("@PK_STORE", SqlDbType.Int).Value = user.PK_STORE;
                            command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;
                            command.CommandText = @"INSERT INTO USERS (USER_NAME, PASSWORD_HASH, PASSWORD_SALT, NAMES, LAST_NAMES, IDENTIFICATION, PHONE, STATUS, PK_ROLE, PK_STORE, CREATION_DATE) 
                                                                 VALUES (@USER_NAME, @PASSWORD_HASH, @PASSWORD_SALT, @NAMES, @LAST_NAMES, @IDENTIFICATION, @PHONE, @STATUS, @PK_ROLE, @PK_STORE, @CREATION_DATE)";
                            await command.ExecuteNonQueryAsync();
                            return "Usuario creado exitosamente.";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public async Task<string> UpdateUser(Users user)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE USERS SET USER_NAME = @USER_NAME, NAMES = @NAMES, LAST_NAMES = @LAST_NAMES, IDENTIFICATION = @IDENTIFICATION, PHONE = @PHONE, PK_ROLE = @PK_ROLE, PK_STORE = @PK_STORE WHERE PK_USER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", user.PK_USER);
                        command.Parameters.Add("@USER_NAME", SqlDbType.NVarChar, 25).Value = user.USER_NAME;
                        command.Parameters.Add("@NAMES", SqlDbType.NVarChar, 30).Value = user.NAMES;
                        command.Parameters.Add("@LAST_NAMES", SqlDbType.NVarChar, 30).Value = user.LAST_NAMES;
                        command.Parameters.Add("@IDENTIFICATION", SqlDbType.VarChar, 10).Value = user.IDENTIFICATION ?? (object)DBNull.Value;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = user.PHONE ?? (object)DBNull.Value;
                        command.Parameters.Add("@PK_ROLE", SqlDbType.Int).Value = user.PK_ROLE;
                        command.Parameters.Add("@PK_STORE", SqlDbType.Int).Value = user.PK_STORE;
                        await command.ExecuteNonQueryAsync();
                        return "Actualización del usuario exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task<string> UpdatePassword(Users user, string password)
        {
            try
            {
                GeneratePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE USERS SET USER_NAME = @USER_NAME, PASSWORD_HASH = @PASSWORD_HASH, PASSWORD_SALT = @PASSWORD_SALT, NAMES = @NAMES, LAST_NAMES = @LAST_NAMES, IDENTIFICATION = @IDENTIFICATION, PHONE = @PHONE, PK_ROLE = @PK_ROLE, PK_STORE = @PK_STORE WHERE PK_USER = @id", connection))
                    {

                        command.Parameters.AddWithValue("@id", user.PK_USER);
                        command.Parameters.Add("@USER_NAME", SqlDbType.NVarChar, 25).Value = user.USER_NAME;
                        command.Parameters.Add("@PASSWORD_HASH", SqlDbType.VarBinary, -1).Value = passwordHash;
                        command.Parameters.Add("@PASSWORD_SALT", SqlDbType.VarBinary, -1).Value = passwordSalt;
                        command.Parameters.Add("@NAMES", SqlDbType.NVarChar, 30).Value = user.NAMES;
                        command.Parameters.Add("@LAST_NAMES", SqlDbType.NVarChar, 30).Value = user.LAST_NAMES;
                        command.Parameters.Add("@IDENTIFICATION", SqlDbType.VarChar, 10).Value = user.IDENTIFICATION ?? (object)DBNull.Value;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = user.PHONE ?? (object)DBNull.Value;
                        command.Parameters.Add("@PK_ROLE", SqlDbType.Int).Value = user.PK_ROLE;
                        command.Parameters.Add("@PK_STORE", SqlDbType.Int).Value = user.PK_STORE;
                        await command.ExecuteNonQueryAsync();
                        return "Actualización de la contraseña exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the password.", ex);
            }
        }

        public async Task EnabledUser(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE USERS SET STATUS = @STATUS WHERE PK_USER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the user.", ex);
            }
        }

        public async Task DisabledUser(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE USERS SET STATUS = @STATUS WHERE PK_USER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "INACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the user.", ex);
            }
        }

        public async Task<Users?> CredentialsUser(string userName)
        {

            Users? user = null;

            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT USERS.PK_USER, USERS.USER_NAME, USERS.PASSWORD_HASH, USERS.PASSWORD_SALT, USERS.PK_ROLE, USERS.PK_STORE, WAREHOUSES.PK_WAREHOUSE, ROLES.ROLE_NAME, STORES.STORE_NAME
                                                                        FROM USERS INNER JOIN ROLES ON USERS.PK_ROLE = ROLES.PK_ROLE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE INNER JOIN WAREHOUSES ON STORES.PK_STORE = WAREHOUSES.PK_STORE
                                                                        WHERE (USERS.USER_NAME = @user) AND (USERS.STATUS <> 'INACTIVO')", connection))
                    {
                        command.Parameters.AddWithValue("@user", userName);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new Users
                                {
                                    PK_USER = Convert.ToInt32(reader["PK_USER"]),
                                    USER_NAME = reader["USER_NAME"].ToString(),
                                    PASSWORD_HASH = (byte[])reader["PASSWORD_HASH"],
                                    PASSWORD_SALT = (byte[])reader["PASSWORD_SALT"],
                                    PK_ROLE = Convert.ToInt32(reader["PK_ROLE"]),
                                    PK_STORE = Convert.ToInt32(reader["PK_STORE"]),

                                    roles = new Roles
                                    {
                                        ROLE_NAME = reader["ROLE_NAME"].ToString()
                                    },
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString(),
                                        warehouses = new Warehouses
                                        {
                                            PK_WAREHOUSE = Convert.ToInt32(reader["PK_WAREHOUSE"]),
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while obtaining user credentials.", ex);
            }

            return user;
        }

        private void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
