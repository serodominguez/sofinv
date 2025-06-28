using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ConnectionData _connectionData;

        public CategoriesRepository(ConnectionData connectionData) 
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Categories>> ReadCategories()
        {
            var categories = new List<Categories>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_CATEGORY, CATEGORY_NAME, CREATION_DATE, STATUS FROM CATEGORIES ORDER BY PK_CATEGORY DESC", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categories.Add(new Categories
                                {
                                    PK_CATEGORY = Convert.ToInt32(reader["PK_CATEGORY"]),
                                    CATEGORY_NAME = reader["CATEGORY_NAME"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading categories.", ex);
            }
        }

        public async Task<IEnumerable<Categories>> SearchCategory(string text)
        {
            var categories = new List<Categories>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_CATEGORY, CATEGORY_NAME, CREATION_DATE, STATUS FROM CATEGORIES WHERE CATEGORY_NAME LIKE '%' + @text + '%' ORDER BY PK_CATEGORY DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categories.Add(new Categories
                                {
                                    PK_CATEGORY = Convert.ToInt32(reader["PK_CATEGORY"]),
                                    CATEGORY_NAME = reader["CATEGORY_NAME"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    STATUS = reader["STATUS"].ToString()
                                });
                            }
                        }
                    }
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching category.", ex);
            }
        }

        public async Task<IEnumerable<Categories>> SelectCategories()
        {
            var categories = new List<Categories>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PK_CATEGORY, CATEGORY_NAME FROM CATEGORIES ORDER BY PK_CATEGORY DESC", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categories.Add(new Categories
                                {
                                    PK_CATEGORY = Convert.ToInt32(reader["PK_CATEGORY"]),
                                    CATEGORY_NAME = reader["CATEGORY_NAME"].ToString(),
                                });
                            }
                        }
                    }
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while selecting categories.", ex);
            }
        }

        public async Task<string> CreateCategory(Categories category)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"INSERT INTO CATEGORIES (CATEGORY_NAME, STATUS, CREATION_DATE) VALUES (@CATEGORY_NAME, @STATUS, @CREATION_DATE)", connection))
                    {
                        command.Parameters.Add("@CATEGORY_NAME", SqlDbType.NVarChar, 40).Value = category.CATEGORY_NAME;
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;
                        await command.ExecuteNonQueryAsync();
                        return "Categoría creado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the category.", ex);
            }
        }

        public async Task<string> UpdateCategory(Categories category)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE CATEGORIES SET CATEGORY_NAME = @CATEGORY_NAME WHERE PK_CATEGORY = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", category.PK_CATEGORY);
                        command.Parameters.Add("@CATEGORY_NAME", SqlDbType.NVarChar, 40).Value = category.CATEGORY_NAME;
                        await command.ExecuteNonQueryAsync();
                        return "Actualización de la categoría exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        public async Task EnabledCategory(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync(); 

                    using (var command = new SqlCommand(@"UPDATE CATEGORIES SET STATUS = @STATUS WHERE PK_CATEGORY = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the category.", ex);
            }
        }

        public async Task DisabledCategory(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"UPDATE CATEGORIES SET STATUS = @STATUS WHERE PK_CATEGORY = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "INACTIVO";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the category.", ex);
            }
        }
    }
}
