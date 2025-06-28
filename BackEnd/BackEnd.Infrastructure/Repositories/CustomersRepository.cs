using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly ConnectionData _connectionData;

        public CustomersRepository(ConnectionData connectionData) 
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Customers>> ReadCustomers()
        {
            var customers = new List<Customers>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(@"SELECT PK_CUSTOMER, NAMES, LAST_NAMES, IDENTIFICATION, PHONE, CREATION_DATE, STATUS FROM CUSTOMERS ORDER BY PK_CUSTOMER DESC", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customers.Add(new Customers
                                {
                                    PK_CUSTOMER = Convert.ToInt32(reader["PK_CUSTOMER"]),
                                    NAMES = reader["NAMES"].ToString(),
                                    LAST_NAMES = reader["LAST_NAMES"].ToString(),
                                    IDENTIFICATION = reader["IDENTIFICATION"].ToString(),
                                    PHONE = reader["PHONE"] != DBNull.Value ? (int?)Convert.ToInt32(reader["PHONE"]) : null,
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading customers.", ex);
            }
        }

        public async Task<IEnumerable<Customers>> SearchCustomer(string text)
        {
            var customers = new List<Customers>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(@"SELECT PK_CUSTOMER, NAMES, LAST_NAMES, IDENTIFICATION, PHONE, CREATION_DATE, STATUS FROM CUSTOMERS 
                                                                    WHERE (NAMES LIKE '%' + @text + '%' OR LAST_NAMES LIKE '%' + @text + '%' OR IDENTIFICATION LIKE '%' + @text + '%')  ORDER BY PK_CUSTOMER DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customers.Add(new Customers
                                {
                                    PK_CUSTOMER = Convert.ToInt32(reader["PK_CUSTOMER"]),
                                    NAMES = reader["NAMES"].ToString(),
                                    LAST_NAMES = reader["LAST_NAMES"].ToString(),
                                    IDENTIFICATION = reader["IDENTIFICATION"].ToString(),
                                    PHONE = reader["PHONE"] != DBNull.Value ? (int?)Convert.ToInt32(reader["PHONE"]) : null,
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching customers.", ex);
            }
        }

        public async Task<IEnumerable<Customers>> SelectCustomers()
        {
            var customers = new List<Customers>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_CUSTOMER, NAMES+' '+LAST_NAMES AS NAMES FROM CUSTOMERS WHERE STATUS <> 'INACTIVO' ORDER BY NAMES", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customers.Add(new Customers
                                {
                                    PK_CUSTOMER = Convert.ToInt32(reader["PK_CUSTOMER"]),
                                    NAMES = reader["NAMES"].ToString(),
                                });
                            }
                        }
                    }
                }
                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while selecting customers.", ex);
            }
        }

        public async Task<string> CreateCustomer(Customers customer)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT COUNT(*) FROM CUSTOMERS WHERE IDENTIFICATION = @IDENTIFICATION", connection))
                    {
                        command.Parameters.Add("@NAMES", SqlDbType.NVarChar, 30).Value = customer.NAMES;
                        command.Parameters.Add("@LAST_NAMES", SqlDbType.NVarChar, 30).Value = customer.LAST_NAMES;
                        command.Parameters.Add("@IDENTIFICATION", SqlDbType.VarChar, 10).Value = customer.IDENTIFICATION ?? (object)DBNull.Value;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = customer.PHONE ?? (object)DBNull.Value;
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;

                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            return "El cliente ya existe.";
                        }
                        else
                        {
                            command.CommandText = @"INSERT INTO CUSTOMERS (NAMES, LAST_NAMES, IDENTIFICATION, PHONE, STATUS, CREATION_DATE) VALUES (@NAMES, @LAST_NAMES, @IDENTIFICATION, @PHONE, @STATUS, @CREATION_DATE)";
                            await command.ExecuteNonQueryAsync();
                            return "Cliente creado exitosamente.";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the brand.", ex);
            }
        }


        public async Task<string> UpdateCustomer(Customers customer)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE CUSTOMERS SET NAMES = @NAMES, LAST_NAMES = @LAST_NAMES, IDENTIFICATION = @IDENTIFICATION, PHONE = @PHONE WHERE PK_CUSTOMER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", customer.PK_CUSTOMER);
                        command.Parameters.Add("@NAMES", SqlDbType.NVarChar, 30).Value = customer.NAMES;
                        command.Parameters.Add("@LAST_NAMES", SqlDbType.NVarChar, 30).Value = customer.LAST_NAMES;
                        command.Parameters.Add("@IDENTIFICATION", SqlDbType.VarChar, 10).Value = customer.IDENTIFICATION ?? (object)DBNull.Value;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = customer.PHONE ?? (object)DBNull.Value;
                        await command.ExecuteNonQueryAsync();
                        return "Actualización del cliente exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the customer.", ex);
            }
        }

        public async Task EnabledCustomer(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE CUSTOMERS SET STATUS = @STATUS WHERE PK_CUSTOMER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the customer.", ex);
            }
        }

        public async Task DisabledCustomer(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE CUSTOMERS SET STATUS = @STATUS WHERE PK_CUSTOMER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "INACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the customer.", ex);
            }
        }
    }
}
