using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class SuppliersRepository : ISuppliersRepository
    {
        private readonly ConnectionData _connectionData;

        public SuppliersRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Suppliers>> ReadSuppliers()
        {
            var suppliers = new List<Suppliers>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_SUPPLIER, COMPANY_NAME, CONTACT, PHONE, EMAIL, CREATION_DATE, STATUS FROM SUPPLIERS ORDER BY PK_SUPPLIER DESC", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                suppliers.Add(new Suppliers
                                {
                                    PK_SUPPLIER = Convert.ToInt32(reader["PK_SUPPLIER"]),
                                    COMPANY_NAME = reader["COMPANY_NAME"].ToString(),
                                    CONTACT = reader["CONTACT"].ToString(),
                                    PHONE = Convert.ToInt32(reader["PHONE"]),
                                    EMAIL = reader["EMAIL"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading suppliers.", ex);
            }
        }

        public async Task<IEnumerable<Suppliers>> SearchSupplier(string text)
        {
            var suppliers = new List<Suppliers>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_SUPPLIER, COMPANY_NAME, CONTACT, PHONE, EMAIL, STATUS, CREATION_DATE FROM SUPPLIERS 
                                                                   WHERE (COMPANY_NAME LIKE '%' + @text + '%' OR CONTACT LIKE '%' + @text + '%' OR PHONE LIKE '%' + @text + '%' ) ORDER BY PK_SUPPLIER DESC ", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                suppliers.Add(new Suppliers
                                {
                                    PK_SUPPLIER = Convert.ToInt32(reader["PK_SUPPLIER"]),
                                    COMPANY_NAME = reader["COMPANY_NAME"].ToString(),
                                    CONTACT = reader["CONTACT"].ToString(),
                                    PHONE = Convert.ToInt32(reader["PHONE"]),
                                    EMAIL = reader["EMAIL"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching supplier.", ex);
            }
        }

        public async Task<IEnumerable<Suppliers>> SelectSuppliers()
        {
            var suppliers = new List<Suppliers>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_SUPPLIER, COMPANY_NAME FROM SUPPLIERS WHERE STATUS <> 'INACTIVO' ORDER BY COMPANY_NAME", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                suppliers.Add(new Suppliers
                                {
                                    PK_SUPPLIER = Convert.ToInt32(reader["PK_SUPPLIER"]),
                                    COMPANY_NAME = reader["COMPANY_NAME"].ToString(),
                                });
                            }
                        }
                    }
                }
                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while selecting suppliers.", ex);
            }
        }

        public async Task<string> CreateSupplier(Suppliers supplier)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"INSERT INTO SUPPLIERS (COMPANY_NAME, CONTACT, PHONE, EMAIL, STATUS, CREATION_DATE) VALUES (@COMPANY_NAME, @CONTACT, @PHONE, @EMAIL, @STATUS, @CREATION_DATE)", connection))
                    {
                        command.Parameters.Add("@COMPANY_NAME", SqlDbType.NVarChar, 50).Value = supplier.COMPANY_NAME;
                        command.Parameters.Add("@CONTACT", SqlDbType.NVarChar, 50).Value = supplier.CONTACT;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = supplier.PHONE;
                        command.Parameters.Add("@EMAIL", SqlDbType.NVarChar, 50).Value = supplier.EMAIL;
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;
                        await command.ExecuteNonQueryAsync();
                        return "Proveedor creado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the supplier.", ex);
            }
        }

        public async Task<string> UpdateSupplier(Suppliers supplier)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE SUPPLIERS SET COMPANY_NAME = @COMPANY_NAME, CONTACT = @CONTACT, PHONE = @PHONE, EMAIL = @EMAIL WHERE PK_SUPPLIER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", supplier.PK_SUPPLIER);
                        command.Parameters.Add("@COMPANY_NAME", SqlDbType.NVarChar, 50).Value = supplier.COMPANY_NAME;
                        command.Parameters.Add("@CONTACT", SqlDbType.NVarChar, 50).Value = supplier.CONTACT;
                        command.Parameters.Add("@PHONE", SqlDbType.Int).Value = supplier.PHONE;
                        command.Parameters.Add("@EMAIL", SqlDbType.NVarChar, 50).Value = supplier.EMAIL;
                        await command.ExecuteNonQueryAsync();
                        return "Actualización del proveedor exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the supplier.", ex);
            }
        }

        public async Task EnabledSupplier(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE SUPPLIERS SET STATUS = @STATUS WHERE PK_SUPPLIER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the supplier.", ex);
            }
        }

        public async Task DisabledSupplier(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE SUPPLIERS SET STATUS = @STATUS WHERE PK_SUPPLIER = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "INACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the supplier.", ex);
            }
        }
    }
}
