using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class GoodsIssueRepository : IGoodsIssueRepository
    {
        private readonly ConnectionData _connectionData;

        public GoodsIssueRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<GoodsIssue>> ReadIssues(int id)
        {
            var issues = new List<GoodsIssue>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT GOODS_ISSUE.PK_ISSUE, GOODS_ISSUE.CODE, GOODS_ISSUE.DATE_SALE, GOODS_ISSUE.DATE_REGISTRATION, GOODS_ISSUE.TYPE_ISSUE, GOODS_ISSUE.TYPE_DOCUMENT, GOODS_ISSUE.DOCUMENT_NUMBER, GOODS_ISSUE.ANNOTATIONS, GOODS_ISSUE.STATUS, GOODS_ISSUE.PK_CUSTOMER, GOODS_ISSUE.PK_USER, GOODS_ISSUE.PK_WAREHOUSE, (CUSTOMERS.NAMES+' '+CUSTOMERS.LAST_NAMES) AS NAMES, WAREHOUSES.WAREHOUSE_NAME, USERS.USER_NAME, STORES.STORE_NAME
                                                                    FROM GOODS_ISSUE INNER JOIN USERS ON GOODS_ISSUE.PK_USER = USERS.PK_USER INNER JOIN CUSTOMERS ON GOODS_ISSUE.PK_CUSTOMER = CUSTOMERS.PK_CUSTOMER INNER JOIN WAREHOUSES ON GOODS_ISSUE.PK_WAREHOUSE = WAREHOUSES.PK_WAREHOUSE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE AND WAREHOUSES.PK_STORE = STORES.PK_STORE
                                                                    WHERE (GOODS_ISSUE.DATE_REGISTRATION BETWEEN DATEADD(DAY, - 30, GETDATE()) AND GETDATE()) AND GOODS_ISSUE.PK_WAREHOUSE = @id ORDER BY GOODS_ISSUE.PK_ISSUE DESC", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                issues.Add(new GoodsIssue
                                {
                                    PK_ISSUE = Convert.ToInt32(reader["PK_ISSUE"]),
                                    CODE = reader["CODE"].ToString(),
                                    DATE_SALE = Convert.ToDateTime(reader["DATE_SALE"]),
                                    DATE_REGISTRATION = Convert.ToDateTime(reader["DATE_REGISTRATION"]),
                                    TYPE_ISSUE = reader["TYPE_ISSUE"].ToString(),
                                    TYPE_DOCUMENT = reader["TYPE_DOCUMENT"].ToString(),
                                    DOCUMENT_NUMBER = reader["DOCUMENT_NUMBER"].ToString(),
                                    ANNOTATIONS = reader["ANNOTATIONS"].ToString(),
                                    STATUS = reader["STATUS"].ToString(),
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString(),
                                    },
                                    customers = new Customers
                                    {
                                        NAMES = reader["NAMES"].ToString(),
                                    },
                                    users = new Users
                                    {
                                        USER_NAME = reader["USER_NAME"].ToString(),
                                    },
                                    warehouses = new Warehouses
                                    {
                                        WAREHOUSE_NAME = reader["WAREHOUSE_NAME"].ToString(),
                                    },
                                });
                            }
                        }
                    }
                }
                return issues;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading goods issue", ex);
            }
        }

        public async Task<IEnumerable<GoodsIssue>> SearchIssue(string text, int id)
        {
            var issues = new List<GoodsIssue>();

            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT GOODS_ISSUE.PK_ISSUE, GOODS_ISSUE.CODE, GOODS_ISSUE.DATE_SALE, GOODS_ISSUE.DATE_REGISTRATION, GOODS_ISSUE.TYPE_ISSUE, GOODS_ISSUE.TYPE_DOCUMENT, GOODS_ISSUE.DOCUMENT_NUMBER, GOODS_ISSUE.ANNOTATIONS, GOODS_ISSUE.STATUS, GOODS_ISSUE.PK_CUSTOMER, GOODS_ISSUE.PK_USER, GOODS_ISSUE.PK_WAREHOUSE, (CUSTOMERS.NAMES+' '+CUSTOMERS.LAST_NAMES) AS NAMES, WAREHOUSES.WAREHOUSE_NAME, USERS.USER_NAME, STORES.STORE_NAME
                                                                    FROM GOODS_ISSUE INNER JOIN USERS ON GOODS_ISSUE.PK_USER = USERS.PK_USER INNER JOIN CUSTOMERS ON GOODS_ISSUE.PK_CUSTOMER = CUSTOMERS.PK_CUSTOMER INNER JOIN WAREHOUSES ON GOODS_ISSUE.PK_WAREHOUSE = WAREHOUSES.PK_WAREHOUSE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE AND WAREHOUSES.PK_STORE = STORES.PK_STORE
                                                                    WHERE (GOODS_ISSUE.CODE LIKE '%' + @text + '%' OR  SUPPLIERS.COMPANY_NAME LIKE '%' + @text + '%' OR USERS.USER_NAME LIKE '%' + @text + '%') AND GOODS_ISSUE.PK_WAREHOUSE = @id ORDER BY GOODS_ISSUE.PK_ISSUE DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                issues.Add(new GoodsIssue
                                {
                                    PK_ISSUE = Convert.ToInt32(reader["PK_ISSUE"]),
                                    CODE = reader["CODE"].ToString(),
                                    DATE_SALE = Convert.ToDateTime(reader["DATE_SALE"]),
                                    DATE_REGISTRATION = Convert.ToDateTime(reader["DATE_REGISTRATION"]),
                                    TYPE_ISSUE = reader["TYPE_ISSUE"].ToString(),
                                    TYPE_DOCUMENT = reader["TYPE_DOCUMENT"].ToString(),
                                    DOCUMENT_NUMBER = reader["DOCUMENT_NUMBER"].ToString(),
                                    ANNOTATIONS = reader["ANNOTATIONS"].ToString(),
                                    STATUS = reader["STATUS"].ToString(),
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString(),
                                    },
                                    customers = new Customers
                                    {
                                        NAMES = reader["NAMES"].ToString(),
                                    },
                                    users = new Users
                                    {
                                        USER_NAME = reader["USER_NAME"].ToString(),
                                    },
                                    warehouses = new Warehouses
                                    {
                                        WAREHOUSE_NAME = reader["WAREHOUSE_NAME"].ToString(),
                                    },
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching goods issue.", ex);
            }
            return issues;
        }

        public async Task<string> CreateIssue(GoodsIssue issue)
        {
            using (var connection = _connectionData.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        if (issue.details != null)
                        {
                            issue.PK_ISSUE = await InsertIssue(connection, transaction, issue);
                            await InsertDetail(connection, transaction, issue);
                        }

                        transaction.Commit();
                        return "Salida creada exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the goods issue.", ex);
                    }
                }
            }
        }

        public async Task DisabledIssue(int id)
        {

            using (var connection = _connectionData.CreateConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand(@"UPDATE GOODS_ISSUE SET STATUS = @STATUS WHERE PK_ISSUE = @id", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ANULADO";
                            await command.ExecuteNonQueryAsync();
                        }

                        await IncreaseQuantity(connection, transaction, id);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while disabling the goods issue.", ex);
                    }
                }
            }
        }

        private async Task<int> InsertIssue(SqlConnection connection, SqlTransaction transaction, GoodsIssue issue)
        {
            DateTime currentDate = DateTime.Now;
            string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");
            string formattedDate = Convert.ToDateTime(issue.DATE_SALE).ToString("yyyy-MM-dd");

            try
            {
                using (var command = new SqlCommand(@"INSERT INTO GOODS_ISSUE (CODE, DATE_SALE, DATE_REGISTRATION, TYPE_ISSUE, TYPE_DOCUMENT, DOCUMENT_NUMBER, ANNOTATIONS, STATUS, PK_CUSTOMER, PK_USER, PK_WAREHOUSE) VALUES (@CODE, @DATE_SALE, @DATE_REGISTRATION, @TYPE_ISSUE, @TYPE_DOCUMENT, @DOCUMENT_NUMBER, @ANNOTATIONS, @STATUS, @PK_CUSTOMER, @PK_USER, @PK_WAREHOUSE);
                                                            SELECT SCOPE_IDENTITY();", connection, transaction))
                {
                    string code = await GenerateCode(connection, transaction, issue.PK_WAREHOUSE);
                    command.Parameters.Add("@CODE", SqlDbType.NVarChar, 15).Value = code;
                    command.Parameters.Add("@DATE_SALE", SqlDbType.Date).Value = formattedDate;
                    command.Parameters.Add("@DATE_REGISTRATION", SqlDbType.DateTime).Value = creationDate;
                    command.Parameters.Add("@TYPE_ISSUE", SqlDbType.VarChar, 15).Value = issue.TYPE_ISSUE;
                    command.Parameters.Add("@TYPE_DOCUMENT", SqlDbType.VarChar, 15).Value = issue.TYPE_DOCUMENT;
                    command.Parameters.Add("@DOCUMENT_NUMBER", SqlDbType.VarChar, 30).Value = issue.DOCUMENT_NUMBER;
                    command.Parameters.Add("@ANNOTATIONS", SqlDbType.NVarChar, 80).Value = issue.ANNOTATIONS;
                    command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                    command.Parameters.Add("@PK_CUSTOMER", SqlDbType.Int).Value = issue.PK_CUSTOMER;
                    command.Parameters.Add("@PK_USER", SqlDbType.Int).Value = issue.PK_USER;
                    command.Parameters.Add("@PK_WAREHOUSE", SqlDbType.Int).Value = issue.PK_WAREHOUSE;

                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting issue.", ex);
            }
        }

        private async Task InsertDetail(SqlConnection connection, SqlTransaction transaction, GoodsIssue issue)
        {
            if (issue?.details == null || !issue.details.Any())
            {
                return;
            }

            try
            {
                int indexNumber = 1;

                foreach (var details in issue.details)
                {
                    using (var command = new SqlCommand(@"INSERT INTO DETAILS_ISSUE (PK_ISSUE, PK_PRODUCT, QUANTITY, PRICE, INDEX_NUMBER) VALUES (@PK_ISSUE, @PK_PRODUCT, @QUANTITY, @PRICE, @INDEX_NUMBER)", connection, transaction))
                    {
                        command.Parameters.Add("@PK_ISSUE", SqlDbType.Int).Value = issue.PK_ISSUE;
                        command.Parameters.Add("@PK_PRODUCT", SqlDbType.Int).Value = details.PK_PRODUCT;
                        command.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = details.QUANTITY;
                        command.Parameters.Add("@PRICE", SqlDbType.Int).Value = details.PRICE;
                        command.Parameters.Add("@INDEX_NUMBER", SqlDbType.Int).Value = indexNumber;

                        await command.ExecuteNonQueryAsync();
                        await DecreaseQuantity(connection, transaction, issue.PK_WAREHOUSE, details.PK_PRODUCT, details.QUANTITY);
                        indexNumber++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting details into the issue.", ex);
            }
        }

        public async Task<IEnumerable<DetailsIssue>> ReadDetails(int id)
        {
            var details = new List<DetailsIssue>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PRODUCTS.CODE, PRODUCTS.DESCRIPTION, PRODUCTS.MATERIAL, PRODUCTS.COLOUR, CATEGORIES.CATEGORY_NAME, BRANDS.BRAND_NAME, DETAILS_ISSUE.QUANTITY, DETAILS_ISSUE.PRICE
                                                            FROM DETAILS_ISSUE INNER JOIN PRODUCTS ON DETAILS_ISSUE.PK_PRODUCT = PRODUCTS.PK_PRODUCT INNER JOIN BRANDS ON PRODUCTS.PK_BRAND = BRANDS.PK_BRAND INNER JOIN CATEGORIES ON PRODUCTS.PK_CATEGORY = CATEGORIES.PK_CATEGORY 
                                                            WHERE DETAILS_ISSUE.PK_ISSUE = @id ORDER BY DETAILS_ISSUE.INDEX_NUMBER", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new DetailsIssue
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
                                    QUANTITY = Convert.ToInt32(reader["QUANTITY"]),
                                    PRICE = Convert.ToInt32(reader["PRICE"]),

                                });
                            }
                        }
                    }
                }
                return details;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading goods issue details", ex);
            }
        }

        private async Task IncreaseQuantity(SqlConnection connection, SqlTransaction transaction, int PK_ISSUE)
        {
            try
            {
                var updates = new List<(int PK_WAREHOUSE, int PK_PRODUCT, int QUANTITY)>();

                using (var command = new SqlCommand(@"SELECT GOODS_ISSUE.PK_WAREHOUSE, DETAILS_ISSUE.PK_PRODUCT, DETAILS_ISSUE.QUANTITY 
                                                            FROM DETAILS_ISSUE INNER JOIN GOODS_ISSUE ON DETAILS_ISSUE.PK_ISSUE = GOODS_ISSUE.PK_ISSUE 
                                                            WHERE GOODS_ISSUE.PK_ISSUE = @PK_ISSUE", connection, transaction))
                {
                    command.Parameters.Add("@PK_ISSUE", SqlDbType.Int).Value = PK_ISSUE;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int PK_WAREHOUSE = Convert.ToInt32(reader["PK_WAREHOUSE"]);
                            int PK_PRODUCT = Convert.ToInt32(reader["PK_PRODUCT"]);
                            int QUANTITY = Convert.ToInt32(reader["QUANTITY"]);

                            updates.Add((PK_WAREHOUSE, PK_PRODUCT, QUANTITY));
                        }
                    }
                }

                foreach (var update in updates)
                {
                    using (var updateCommand = new SqlCommand(@"UPDATE DETAILS_WAREHOUSE SET STOCK = STOCK + @QUANTITY WHERE PK_PRODUCT = @PK_PRODUCT AND PK_WAREHOUSE = @PK_WAREHOUSE", connection, transaction))
                    {
                        updateCommand.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = update.QUANTITY;
                        updateCommand.Parameters.Add("@PK_PRODUCT", SqlDbType.Int).Value = update.PK_PRODUCT;
                        updateCommand.Parameters.Add("@PK_WAREHOUSE", SqlDbType.Int).Value = update.PK_WAREHOUSE;
                        await updateCommand.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while decreasing the quantity.", ex);
            }
        }

        private async Task DecreaseQuantity(SqlConnection connection, SqlTransaction transaction, int PK_WAREHOUSE, int PK_PRODUCT, int QUANTITY)
        {
            try
            {
                using (var command = new SqlCommand(@"SELECT COUNT(*) FROM DETAILS_WAREHOUSE WHERE PK_PRODUCT = @PK_PRODUCT AND PK_WAREHOUSE = @PK_WAREHOUSE", connection, transaction))
                {
                    command.Parameters.Add("@PK_WAREHOUSE", SqlDbType.Int).Value = PK_WAREHOUSE;
                    command.Parameters.Add("@PK_PRODUCT", SqlDbType.Int).Value = PK_PRODUCT;

                    if (Convert.ToInt32(await command.ExecuteScalarAsync()) > 0)
                    {
                        command.CommandText = @"UPDATE DETAILS_WAREHOUSE SET STOCK = STOCK - @QUANTITY WHERE PK_PRODUCT = @PK_PRODUCT AND PK_WAREHOUSE = @PK_WAREHOUSE";
                        command.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = QUANTITY;
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO DETAILS_WAREHOUSE (PK_WAREHOUSE, PK_PRODUCT, STOCK, PRICE) VALUES (@PK_WAREHOUSE, @PK_PRODUCT, @STOCK, @PRICE)";
                        command.Parameters.Add("@STOCK", SqlDbType.Int).Value = QUANTITY * -1;
                        command.Parameters.Add("@PRICE", SqlDbType.Int).Value = 0;
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while increasing the quantity.", ex);
            }
        }

        private async Task<string> GenerateCode(SqlConnection connection, SqlTransaction transaction, int PK_WAREHOUSE)
        {
            try
            {
                string code = "";

                using (var command = new SqlCommand("SELECT COUNT(*) + 1 AS CODE FROM GOODS_ISSUE WHERE PK_WAREHOUSE = @id", connection, transaction))
                {
                    command.Parameters.AddWithValue("@id", PK_WAREHOUSE);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            code = "SAL-" + reader["CODE"].ToString();
                        }
                    }
                }
                return code;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the code.", ex);
            }
        }
    }


}
