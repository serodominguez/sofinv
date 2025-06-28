using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class StoresRepository : IStoresRepository
    {
        private readonly ConnectionData _connectionData;

        public StoresRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Stores>> ReadStores()
        {
            var stores = new List<Stores>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_STORE, STORE_NAME, MANAGER, ADDRESS, PHONE, CITY, EMAIL, TYPE, CREATION_DATE, STATUS FROM STORES ORDER BY PK_STORE DESC", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                stores.Add(new Stores
                                {
                                    PK_STORE = Convert.ToInt32(reader["PK_STORE"]),
                                    STORE_NAME = reader["STORE_NAME"].ToString(),
                                    MANAGER = reader["MANAGER"].ToString(),
                                    ADDRESS = reader["ADDRESS"].ToString(),
                                    PHONE = Convert.ToInt32(reader["PHONE"]),
                                    CITY = reader["CITY"].ToString(),
                                    EMAIL = reader["EMAIL"].ToString(),
                                    TYPE = reader["TYPE"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return stores;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading stores.", ex);
            }
        }

        public async Task<IEnumerable<Stores>> SearchStore(string text)
        {
            var stores = new List<Stores>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_STORE, STORE_NAME, MANAGER, ADDRESS, PHONE, CITY, EMAIL, TYPE, CREATION_DATE, STATUS FROM STORES 
                                                                WHERE (STORE_NAME LIKE '%' + @text + '%' OR MANAGER LIKE '%' + @text + '%' OR CITY LIKE '%' + @text + '%') AND STATUS <> 'INACTIVO' ORDER BY PK_STORE DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                stores.Add(new Stores
                                {
                                    PK_STORE = Convert.ToInt32(reader["PK_STORE"]),
                                    STORE_NAME = reader["STORE_NAME"].ToString(),
                                    MANAGER = reader["MANAGER"].ToString(),
                                    ADDRESS = reader["ADDRESS"].ToString(),
                                    PHONE = Convert.ToInt32(reader["PHONE"]),
                                    CITY = reader["CITY"].ToString(),
                                    EMAIL = reader["EMAIL"].ToString(),
                                    TYPE = reader["TYPE"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return stores;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching store.", ex);
            }
        }

        public async Task<IEnumerable<Stores>> SelectStores()
        {
            var stores = new List<Stores>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_STORE, STORE_NAME FROM STORES WHERE STATUS <> 'INACTIVO' ORDER BY STORE_NAME", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                stores.Add(new Stores
                                {
                                    PK_STORE = Convert.ToInt32(reader["PK_STORE"]),
                                    STORE_NAME = reader["STORE_NAME"].ToString(),
                                });
                            }
                        }
                    }
                }
                return stores;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while selecting stores.", ex);
            }
        }

        public async Task<string> CreateStore(Stores store)
        {
            using (var connection = _connectionData.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        store.PK_STORE = await InsertStore(connection, transaction, store);
                        await CreateWarehouse(connection, transaction, store);

                        transaction.Commit();
                        return "Sucursal creado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the store.", ex);
                    }
                }
            }
        }

        public async Task<string> UpdateStore(Stores store)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE STORES SET STORE_NAME = @STORE_NAME, MANAGER = @MANAGER, ADDRESS = @ADDRESS, PHONE = @PHONE, CITY = @CITY, EMAIL = @EMAIL WHERE PK_STORE = @id", connection))
                    {
                        command.Parameters.Add("@STORE_NAME", SqlDbType.NVarChar, 30).Value = store.STORE_NAME;
                        command.Parameters.Add("@MANAGER", SqlDbType.NVarChar, 50).Value = store.MANAGER;
                        command.Parameters.Add("@ADDRESS", SqlDbType.NVarChar, 50).Value = store.ADDRESS;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = store.PHONE;
                        command.Parameters.Add("@CITY", SqlDbType.VarChar, 20).Value = store.CITY;
                        command.Parameters.Add("@EMAIL", SqlDbType.NVarChar, 50).Value = store.EMAIL;
                        command.Parameters.AddWithValue("@id", store.PK_STORE);
                        await command.ExecuteNonQueryAsync();
                        if (string.IsNullOrWhiteSpace(store.STORE_NAME))
                        {
                            throw new ArgumentException("STORE_NAME cannot be null or empty.", nameof(store.STORE_NAME));
                        }
                        await UpdateWarehouse(store.PK_STORE, store.STORE_NAME);
                        return "Actualización de la sucursal exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the store.", ex);
            }
        }

        public async Task EnabledStore(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE STORES SET STATUS = @STATUS WHERE PK_STORE = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.NVarChar, 10).Value = "ACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the store.", ex);
            }
        }

        public async Task DisabledStore(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE STORES SET STATUS = @STATUS WHERE PK_STORE = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.NVarChar, 10).Value = "INACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the store.", ex);
            }
        }

        private async Task<int> InsertStore(SqlConnection connection, SqlTransaction transaction, Stores store)
        {
            DateTime currentDate = DateTime.Now;
            string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

            try
            {
                using (var command = new SqlCommand(@"INSERT INTO STORES (STORE_NAME, MANAGER, ADDRESS, PHONE, CITY, EMAIL, TYPE, CREATION_DATE, STATUS) VALUES (@STORE_NAME, @MANAGER, @ADDRESS, @PHONE, @CITY, @EMAIL, @TYPE, @CREATION_DATE, @STATUS);
                                                                    SELECT SCOPE_IDENTITY();", connection, transaction))
                {
                    command.Parameters.Add("@STORE_NAME", SqlDbType.NVarChar, 30).Value = store.STORE_NAME;
                    command.Parameters.Add("@MANAGER", SqlDbType.NVarChar, 50).Value = store.MANAGER;
                    command.Parameters.Add("@ADDRESS", SqlDbType.NVarChar, 50).Value = store.ADDRESS;
                    command.Parameters.Add("@PHONE", SqlDbType.Int).Value = store.PHONE;
                    command.Parameters.Add("@CITY", SqlDbType.VarChar, 20).Value = store.CITY;
                    command.Parameters.Add("@EMAIL", SqlDbType.NVarChar, 50).Value = store.EMAIL;
                    command.Parameters.Add("@TYPE", SqlDbType.VarChar, 10).Value = "SUCURSAL";
                    command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                    command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting store.", ex);
            }
        }

        private async Task CreateWarehouse(SqlConnection connection, SqlTransaction transaction, Stores store)
        {
            DateTime currentDate = DateTime.Now;
            string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

            try
            {
                using (var command = new SqlCommand(@"INSERT INTO WAREHOUSES (WAREHOUSE_NAME, TYPE_WAREHOUSE, PK_STORE, STATUS, CREATION_DATE) VALUES (@WAREHOUSE_NAME, @TYPE_WAREHOUSE, @PK_STORE, @STATUS, @CREATION_DATE)", connection, transaction))
                {
                    command.Parameters.Add("@WAREHOUSE_NAME", SqlDbType.NVarChar, 30).Value = store.STORE_NAME;
                    command.Parameters.Add("@TYPE_WAREHOUSE", SqlDbType.VarChar, 10).Value = "INTERNO";
                    command.Parameters.Add("@PK_STORE", SqlDbType.Int).Value = store.PK_STORE;
                    command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                    command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating warehouse.", ex);
            }
        }

        private async Task UpdateWarehouse(int PK_STORE, string STORE_NAME)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("UPDATE WAREHOUSES SET WAREHOUSE_NAME = @WAREHOUSE_NAME WHERE PK_STORE = @PK_STORE", connection))
                    {
                        command.Parameters.Add("@PK_STORE", SqlDbType.Int).Value = PK_STORE;
                        command.Parameters.Add("@WAREHOUSE_NAME", SqlDbType.NVarChar, 30).Value = STORE_NAME;
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating warehouse.", ex);
            }
        }

    }
}
