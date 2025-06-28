using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ConnectionData _connectionData;

        public ProductsRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<Products>> ReadProducts()
        {
            var products = new List<Products>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PRODUCTS.PK_PRODUCT, PRODUCTS.CODE, PRODUCTS.DESCRIPTION, PRODUCTS.MEASUREMENT, PRODUCTS.MATERIAL, PRODUCTS.COLOUR, PRODUCTS.STATUS, PRODUCTS.CREATION_DATE, BRANDS.PK_BRAND, BRANDS.BRAND_NAME, CATEGORIES.PK_CATEGORY, CATEGORIES.CATEGORY_NAME FROM BRANDS INNER JOIN PRODUCTS ON BRANDS.PK_BRAND = PRODUCTS.PK_BRAND INNER JOIN CATEGORIES ON PRODUCTS.PK_CATEGORY = CATEGORIES.PK_CATEGORY ORDER BY PRODUCTS.PK_PRODUCT DESC", connection))
                    {

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new Products
                                {
                                    PK_PRODUCT = Convert.ToInt32(reader["PK_PRODUCT"]),
                                    CODE = reader["CODE"].ToString(),
                                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                                    MEASUREMENT = reader["MEASUREMENT"].ToString(),
                                    MATERIAL = reader["MATERIAL"].ToString(),
                                    COLOUR = reader["COLOUR"].ToString(),
                                    STATUS = reader["STATUS"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    PK_BRAND = Convert.ToInt32(reader["PK_BRAND"]),
                                    brands = new Brands
                                    {
                                        BRAND_NAME = reader["BRAND_NAME"].ToString()
                                    },
                                    PK_CATEGORY = Convert.ToInt32(reader["PK_CATEGORY"]),
                                    categories = new Categories
                                    {
                                        CATEGORY_NAME = reader["CATEGORY_NAME"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading products.", ex);
            }

            return products;
        }

        public async Task<IEnumerable<Products>> SearchProduct(string text)
        {
            var products = new List<Products>();

            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PRODUCTS.PK_PRODUCT, PRODUCTS.CODE, PRODUCTS.DESCRIPTION, PRODUCTS.MEASUREMENT, PRODUCTS.MATERIAL, PRODUCTS.COLOUR, PRODUCTS.STATUS, PRODUCTS.CREATION_DATE, BRANDS.PK_BRAND, BRANDS.BRAND_NAME, CATEGORIES.PK_CATEGORY, CATEGORIES.CATEGORY_NAME
                                                                    FROM BRANDS INNER JOIN PRODUCTS ON BRANDS.PK_BRAND = PRODUCTS.PK_BRAND INNER JOIN CATEGORIES ON PRODUCTS.PK_CATEGORY = CATEGORIES.PK_CATEGORY
                                                                    WHERE (PRODUCTS.CODE LIKE '%' + @text + '%' OR CATEGORIES.CATEGORY_NAME LIKE '%' + @text + '%' OR BRANDS.BRAND_NAME LIKE '%' + @text + '%') AND PRODUCTS.STATUS = 'ACTIVO' ORDER BY PRODUCTS.PK_PRODUCT DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new Products
                                {
                                    PK_PRODUCT = Convert.ToInt32(reader["PK_PRODUCT"]),
                                    CODE = reader["CODE"].ToString(),
                                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                                    MEASUREMENT = reader["MEASUREMENT"].ToString(),
                                    MATERIAL = reader["MATERIAL"].ToString(),
                                    COLOUR = reader["COLOUR"].ToString(),
                                    STATUS = reader["STATUS"].ToString(),
                                    CREATION_DATE = Convert.ToDateTime(reader["CREATION_DATE"]),
                                    PK_BRAND = Convert.ToInt32(reader["PK_BRAND"]),
                                    brands = new Brands
                                    {
                                        BRAND_NAME = reader["BRAND_NAME"].ToString()
                                    },
                                    PK_CATEGORY = Convert.ToInt32(reader["PK_CATEGORY"]),
                                    categories = new Categories
                                    {
                                        CATEGORY_NAME = reader["CATEGORY_NAME"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching product.", ex);
            }
            return products;
        }

        public async Task<string> CreateProduct(Products product)
        {
            using (var connection = _connectionData.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        if (product.storing != null)
                        {
                            product.PK_PRODUCT = await InsertProduct(connection, transaction, product);
                            await InsertWarehouses(connection, transaction, product);
                        }

                        transaction.Commit();
                        return "Producto creado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the product.", ex);
                    }
                }
            }
        }

        public async Task<string> UpdateProduct(Products product)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(@"UPDATE PRODUCTS SET CODE = @CODE, DESCRIPTION = @DESCRIPTION, MATERIAL = @MATERIAL, COLOUR = @COLOUR, MEASUREMENT = @MEASUREMENT, PK_BRAND = @PK_BRAND, PK_CATEGORY = @PK_CATEGORY 
                                                                    WHERE PK_PRODUCT = @id", connection))
                    {
                        command.Parameters.Add("@CODE", SqlDbType.NVarChar, 25).Value = product.CODE;
                        command.Parameters.Add("@DESCRIPTION", SqlDbType.NVarChar, 50).Value = product.DESCRIPTION;
                        command.Parameters.Add("@MEASUREMENT", SqlDbType.VarChar, 15).Value = product.MEASUREMENT;
                        command.Parameters.Add("@MATERIAL", SqlDbType.VarChar, 20).Value = product.MATERIAL;
                        command.Parameters.Add("@COLOUR", SqlDbType.VarChar, 20).Value = product.COLOUR;
                        command.Parameters.Add("@PK_BRAND", SqlDbType.Int).Value = product.PK_BRAND;
                        command.Parameters.Add("@PK_CATEGORY", SqlDbType.Int).Value = product.PK_CATEGORY;
                        command.Parameters.AddWithValue("@id", product.PK_PRODUCT); 
                        await command.ExecuteNonQueryAsync();
                        return "Actualización del producto exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        public async Task EnabledProduct(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(@"UPDATE PRODUCTS SET STATUS = @STATUS WHERE PK_PRODUCT = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while enabling the product.", ex);
            }
        }

        public async Task DisabledProduct(int id)
        {
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(@"UPDATE PRODUCTS SET STATUS = @STATUS WHERE PK_PRODUCT = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "INACTIVO";

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while disabling the product.", ex);
            }
        }

        private async Task<int> InsertProduct(SqlConnection connection, SqlTransaction transaction, Products product)
        {
            DateTime currentDate = DateTime.Now;
            string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");

            using (var command = new SqlCommand(@"INSERT INTO PRODUCTS (CODE, DESCRIPTION, MEASUREMENT, MATERIAL, COLOUR, STATUS, PK_BRAND, PK_CATEGORY, CREATION_DATE) VALUES (@CODE, @DESCRIPTION, @MEASUREMENT, @MATERIAL, @COLOUR, @STATUS, @PK_BRAND, @PK_CATEGORY, @CREATION_DATE);
                                                            SELECT SCOPE_IDENTITY();", connection, transaction))
            {
                command.Parameters.Add("@CODE", SqlDbType.NVarChar, 25).Value = product.CODE;
                command.Parameters.Add("@DESCRIPTION", SqlDbType.NVarChar, 50).Value = product.DESCRIPTION;
                command.Parameters.Add("@MEASUREMENT", SqlDbType.VarChar, 15).Value = product.MEASUREMENT;
                command.Parameters.Add("@MATERIAL", SqlDbType.VarChar, 20).Value = product.MATERIAL;
                command.Parameters.Add("@COLOUR", SqlDbType.VarChar, 20).Value = product.COLOUR;
                command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                command.Parameters.Add("@PK_BRAND", SqlDbType.Int).Value = product.PK_BRAND;
                command.Parameters.Add("@PK_CATEGORY", SqlDbType.Int).Value = product.PK_CATEGORY;
                command.Parameters.Add("@CREATION_DATE", SqlDbType.DateTime).Value = creationDate;

                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        private async Task InsertWarehouses(SqlConnection connection, SqlTransaction transaction, Products product)
        {
            if (product?.storing == null || !product.storing.Any())
            {
                return;
            }

            var warehouses = new List<int>();

            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;

                command.CommandText = @"SELECT PK_WAREHOUSE FROM WAREHOUSES";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        warehouses.Add(Convert.ToInt32(reader["PK_WAREHOUSE"]));
                    }
                }
            }

            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;

                var price = product.storing.FirstOrDefault()?.PRICE;

                foreach (var warehouseId in warehouses)
                {
                    command.Parameters.Clear();
                    command.CommandText = @"INSERT INTO DETAILS_WAREHOUSE (PK_WAREHOUSE, PK_PRODUCT, PRICE, STOCK) VALUES (@PK_WAREHOUSE, @PK_PRODUCT, @PRICE, @STOCK)";
                    command.Parameters.Add("@PK_WAREHOUSE", SqlDbType.Int).Value = warehouseId;
                    command.Parameters.Add("@PK_PRODUCT", SqlDbType.Int).Value = product.PK_PRODUCT;
                    command.Parameters.Add("@PRICE", SqlDbType.Int).Value = price;
                    command.Parameters.Add("@STOCK", SqlDbType.Int).Value = 0;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
