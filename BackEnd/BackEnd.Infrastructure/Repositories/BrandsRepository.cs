using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly ConnectionData _connectionData;

        public BrandsRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Brands>> ReadBrands()
        {
            var brands = new List<Brands>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_BRAND, BRAND_NAME, CREATION_DATE, STATUS FROM BRANDS ORDER BY PK_BRAND DESC", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                brands.Add(new Brands
                                {
                                    PK_BRAND = Convert.ToInt32(reader["PK_BRAND"]),
                                    BRAND_NAME = reader["BRAND_NAME"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return brands;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading brands.", ex);
            }
        }

        public async Task<IEnumerable<Brands>> SearchBrand(string text)
        {
            var brands = new List<Brands>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync(); 

                    using (var command = new SqlCommand(@"SELECT PK_BRAND, BRAND_NAME, CREATION_DATE, STATUS FROM BRANDS WHERE BRAND_NAME LIKE '%' + @text + '%' ORDER BY PK_BRAND DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                brands.Add(new Brands
                                {
                                    PK_BRAND = Convert.ToInt32(reader["PK_BRAND"]),
                                    BRAND_NAME = reader["BRAND_NAME"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return brands;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching brand.", ex);
            }
        }

        public async Task<IEnumerable<Brands>> SelectBrands()
        {
            var brands = new List<Brands>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_BRAND, BRAND_NAME FROM BRANDS WHERE STATUS <> 'INACTIVO' ORDER BY BRAND_NAME", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                brands.Add(new Brands
                                {
                                    PK_BRAND = Convert.ToInt32(reader["PK_BRAND"]),
                                    BRAND_NAME = reader["BRAND_NAME"].ToString(),
                                });
                            }
                        }
                    }
                }
                return brands;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while selecting brands.", ex);
            }
        }

        public async Task<string> CreateBrand(Brands brand)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"INSERT INTO BRANDS (BRAND_NAME, STATUS, CREATION_DATE) VALUES (@BRAND_NAME, @STATUS, @CREATION_DATE)", connection))
                    {
                        command.Parameters.Add("@BRAND_NAME", SqlDbType.NVarChar, 40).Value = brand.BRAND_NAME;
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;
                        await command.ExecuteNonQueryAsync();
                        return "Marca creado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the brand.", ex);
            }
        }

        public async Task<string> UpdateBrand(Brands brand)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync(); 

                    using (var command = new SqlCommand(@"UPDATE BRANDS SET BRAND_NAME = @BRAND_NAME WHERE PK_BRAND = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", brand.PK_BRAND);
                        command.Parameters.Add("@BRAND_NAME", SqlDbType.NVarChar, 40).Value = brand.BRAND_NAME;
                        await command.ExecuteNonQueryAsync();
                        return "Actualización de la marca exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the brand.", ex);
            }
        }

        public async Task EnabledBrand(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE BRANDS SET STATUS = @STATUS WHERE PK_BRAND = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the brand.", ex);
            }
        }

        public async Task DisabledBrand(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE BRANDS SET STATUS = @STATUS WHERE PK_BRAND = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "INACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the brand.", ex);
            }
        }
    }
}
