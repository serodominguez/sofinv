using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class WarehousesRepository : IWarehousesRepository
    {
        private readonly ConnectionData _connectionData;

        public WarehousesRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<DetailsWarehouse>> SearchStock(string text, int id)
        {
            var details = new List<DetailsWarehouse>();

            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PRODUCTS.CODE, PRODUCTS.DESCRIPTION, PRODUCTS.MATERIAL, PRODUCTS.COLOUR, DETAILS_WAREHOUSE.PK_PRODUCT, DETAILS_WAREHOUSE.STOCK, DETAILS_WAREHOUSE.PRICE, BRANDS.BRAND_NAME, CATEGORIES.CATEGORY_NAME
                                                                    FROM DETAILS_WAREHOUSE INNER JOIN PRODUCTS ON DETAILS_WAREHOUSE.PK_PRODUCT = PRODUCTS.PK_PRODUCT INNER JOIN BRANDS ON PRODUCTS.PK_BRAND = BRANDS.PK_BRAND INNER JOIN CATEGORIES ON PRODUCTS.PK_CATEGORY = CATEGORIES.PK_CATEGORY
                                                                    WHERE (PRODUCTS.CODE LIKE '%' + @text + '%' OR DETAILS_WAREHOUSE.PRICE LIKE '%' + @text + '%' OR CATEGORIES.CATEGORY_NAME LIKE '%' + @text + '%' OR BRANDS.BRAND_NAME LIKE '%' + @text + '%') AND PK_WAREHOUSE = @id
                                                                    ORDER BY PRODUCTS.PK_PRODUCT DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new DetailsWarehouse
                                {
                                    products = new Products
                                    {
                                        CODE = reader["CODE"].ToString(),
                                        DESCRIPTION = reader["DESCRIPTION"].ToString(),
                                        MATERIAL = reader["MATERIAL"].ToString(),
                                        COLOUR = reader["COLOUR"].ToString(),
                                        brands = new Brands
                                        {
                                            BRAND_NAME = reader["BRAND_NAME"].ToString()
                                        },
                                        categories = new Categories
                                        {
                                            CATEGORY_NAME = reader["CATEGORY_NAME"].ToString()
                                        }
                                    },
                                    PK_PRODUCT = Convert.ToInt32(reader["PK_PRODUCT"]),
                                    STOCK = Convert.ToInt32(reader["STOCK"]),
                                    PRICE = Convert.ToInt32(reader["PRICE"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching stock.", ex);
            }
            return details;
        }

        public async Task<IEnumerable<DetailsWarehouse>> SelectProduct(string text, int id)
        {
            var details = new List<DetailsWarehouse>();

            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PRODUCTS.PK_PRODUCT, PRODUCTS.CODE, PRODUCTS.DESCRIPTION, PRODUCTS.MATERIAL, PRODUCTS.COLOUR, BRANDS.BRAND_NAME, CATEGORIES.CATEGORY_NAME, COALESCE(DWL.STOCK, 0) AS STOCK, COALESCE(DWL.PRICE, 0) AS PRICE
                                                                    FROM PRODUCTS LEFT JOIN  DETAILS_WAREHOUSE DWL ON PRODUCTS.PK_PRODUCT = DWL.PK_PRODUCT AND DWL.PK_WAREHOUSE = @id LEFT JOIN 
                                                                            (SELECT DISTINCT PK_WAREHOUSE FROM DETAILS_WAREHOUSE) DW ON DWL.PK_WAREHOUSE = DW.PK_WAREHOUSE INNER JOIN BRANDS ON PRODUCTS.PK_BRAND = BRANDS.PK_BRAND INNER JOIN CATEGORIES ON PRODUCTS.PK_CATEGORY = CATEGORIES.PK_CATEGORY
                                                                    WHERE (PRODUCTS.CODE LIKE '%' + @text + '%' OR CATEGORIES.CATEGORY_NAME LIKE '%' + @text + '%' OR BRANDS.BRAND_NAME LIKE '%' + @text + '%') ORDER BY PRODUCTS.PK_PRODUCT DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new DetailsWarehouse
                                {
                                    products = new Products
                                    {
                                        CODE = reader["CODE"].ToString(),
                                        DESCRIPTION = reader["DESCRIPTION"].ToString(),
                                        MATERIAL = reader["MATERIAL"].ToString(),
                                        COLOUR = reader["COLOUR"].ToString(),
                                        brands = new Brands
                                        {
                                            BRAND_NAME = reader["BRAND_NAME"].ToString()
                                        },
                                        categories = new Categories
                                        {
                                            CATEGORY_NAME = reader["CATEGORY_NAME"].ToString()
                                        }
                                    },
                                    PK_PRODUCT = Convert.ToInt32(reader["PK_PRODUCT"]),
                                    STOCK = Convert.ToInt32(reader["STOCK"]),
                                    PRICE = Convert.ToInt32(reader["PRICE"]),
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
            return details;
        }
    }
}
