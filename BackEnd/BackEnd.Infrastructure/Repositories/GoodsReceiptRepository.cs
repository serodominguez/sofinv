using System.Data;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;
using BackEnd.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace BackEnd.Infrastructure.Repositories
{
    public class GoodsReceiptRepository : IGoodsReceiptRepository
    {
        private readonly ConnectionData _connectionData;

        public GoodsReceiptRepository(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public async Task<IEnumerable<GoodsReceipt>> ReadReceipts(int id)
        {
            var receipts = new List<GoodsReceipt>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT GOODS_RECEIPT.PK_RECEIPT, GOODS_RECEIPT.CODE, GOODS_RECEIPT.DATE_PURCHASE, GOODS_RECEIPT.DATE_REGISTRATION, GOODS_RECEIPT.TYPE_RECEIPT, GOODS_RECEIPT.TYPE_DOCUMENT, GOODS_RECEIPT.DOCUMENT_NUMBER, GOODS_RECEIPT.ANNOTATIONS, GOODS_RECEIPT.STATUS, GOODS_RECEIPT.PK_SUPPLIER, GOODS_RECEIPT.PK_USER, GOODS_RECEIPT.PK_WAREHOUSE, SUPPLIERS.COMPANY_NAME, WAREHOUSES.WAREHOUSE_NAME, USERS.USER_NAME, STORES.STORE_NAME
                                                                    FROM GOODS_RECEIPT INNER JOIN USERS ON GOODS_RECEIPT.PK_USER = USERS.PK_USER INNER JOIN SUPPLIERS ON GOODS_RECEIPT.PK_SUPPLIER = SUPPLIERS.PK_SUPPLIER INNER JOIN WAREHOUSES ON GOODS_RECEIPT.PK_WAREHOUSE = WAREHOUSES.PK_WAREHOUSE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE
                                                                    WHERE (GOODS_RECEIPT.DATE_REGISTRATION BETWEEN DATEADD(DAY, - 30, GETDATE()) AND GETDATE()) AND GOODS_RECEIPT.PK_WAREHOUSE = @id ORDER BY GOODS_RECEIPT.PK_RECEIPT DESC", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                receipts.Add(new GoodsReceipt
                                {
                                    PK_RECEIPT = Convert.ToInt32(reader["PK_RECEIPT"]),
                                    CODE = reader["CODE"].ToString(),
                                    DATE_PURCHASE = Convert.ToDateTime(reader["DATE_PURCHASE"]),
                                    DATE_REGISTRATION = Convert.ToDateTime(reader["DATE_REGISTRATION"]),
                                    TYPE_RECEIPT = reader["TYPE_RECEIPT"].ToString(),
                                    TYPE_DOCUMENT = reader["TYPE_DOCUMENT"].ToString(),
                                    DOCUMENT_NUMBER = reader["DOCUMENT_NUMBER"].ToString(),
                                    ANNOTATIONS = reader["ANNOTATIONS"].ToString(),
                                    STATUS = reader["STATUS"].ToString(),
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString(),
                                    },
                                    suppliers = new Suppliers
                                    {
                                        COMPANY_NAME = reader["COMPANY_NAME"].ToString(),
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
                return receipts;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading goods receipt", ex);
            }
        }

        public async Task<IEnumerable<GoodsReceipt>> SearchReceipt(string text, int id)
        {
            var receipts = new List<GoodsReceipt>();

            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT GOODS_RECEIPT.PK_RECEIPT, GOODS_RECEIPT.CODE, GOODS_RECEIPT.DATE_PURCHASE, GOODS_RECEIPT.DATE_REGISTRATION, GOODS_RECEIPT.TYPE_RECEIPT, GOODS_RECEIPT.TYPE_DOCUMENT, GOODS_RECEIPT.DOCUMENT_NUMBER, GOODS_RECEIPT.ANNOTATIONS, GOODS_RECEIPT.STATUS, GOODS_RECEIPT.PK_SUPPLIER, GOODS_RECEIPT.PK_USER, GOODS_RECEIPT.PK_WAREHOUSE, SUPPLIERS.COMPANY_NAME, WAREHOUSES.WAREHOUSE_NAME, USERS.USER_NAME, STORES.STORE_NAME
                                                                    FROM GOODS_RECEIPT INNER JOIN USERS ON GOODS_RECEIPT.PK_USER = USERS.PK_USER INNER JOIN SUPPLIERS ON GOODS_RECEIPT.PK_SUPPLIER = SUPPLIERS.PK_SUPPLIER INNER JOIN WAREHOUSES ON GOODS_RECEIPT.PK_WAREHOUSE = WAREHOUSES.PK_WAREHOUSE INNER JOIN STORES ON USERS.PK_STORE = STORES.PK_STORE
                                                                    WHERE (GOODS_RECEIPT.CODE LIKE '%' + @text + '%' OR  SUPPLIERS.COMPANY_NAME LIKE '%' + @text + '%' OR USERS.USER_NAME LIKE '%' + @text + '%') AND GOODS_RECEIPT.PK_WAREHOUSE = @id ORDER BY GOODS_RECEIPT.PK_RECEIPT DESC", connection))
                    {
                        command.Parameters.AddWithValue("@text", text);
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                receipts.Add(new GoodsReceipt
                                {
                                    PK_RECEIPT = Convert.ToInt32(reader["PK_RECEIPT"]),
                                    CODE = reader["CODE"].ToString(),
                                    DATE_PURCHASE = Convert.ToDateTime(reader["DATE_PURCHASE"]),
                                    DATE_REGISTRATION = Convert.ToDateTime(reader["DATE_REGISTRATION"]),
                                    TYPE_RECEIPT = reader["TYPE_RECEIPT"].ToString(),
                                    TYPE_DOCUMENT = reader["TYPE_DOCUMENT"].ToString(),
                                    DOCUMENT_NUMBER = reader["DOCUMENT_NUMBER"].ToString(),
                                    ANNOTATIONS = reader["ANNOTATIONS"].ToString(),
                                    STATUS = reader["STATUS"].ToString(),
                                    stores = new Stores
                                    {
                                        STORE_NAME = reader["STORE_NAME"].ToString(),
                                    },
                                    suppliers = new Suppliers
                                    {
                                        COMPANY_NAME = reader["COMPANY_NAME"].ToString(),
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
                throw new Exception("An error occurred while searching goods receipt.", ex);
            }
            return receipts;
        }

        public async Task<string> CreateReceipt(GoodsReceipt receipt)
        {
            using (var connection = _connectionData.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        if (receipt.details != null)
                        {
                            receipt.PK_RECEIPT = await InsertReceipt(connection, transaction, receipt);
                            await InsertDetail(connection, transaction, receipt);
                        }

                        transaction.Commit();
                        return "Entrada creado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the goods receipt.", ex);
                    }
                }
            }
        }

        public async Task DisabledReceipt(int id)
        {
            using (var connection = _connectionData.CreateConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand(@"UPDATE GOODS_RECEIPT SET STATUS = @STATUS WHERE PK_RECEIPT = @id", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ANULADO";
                            await command.ExecuteNonQueryAsync();
                        }

                        await DecreaseQuantity(connection, transaction, id);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while disabling the goods receipt.", ex);
                    }
                }
            }
        }

        private async Task<int> InsertReceipt(SqlConnection connection, SqlTransaction transaction, GoodsReceipt receipt)
        {
            DateTime currentDate = DateTime.Now;
            string creationDate = currentDate.ToString("yyyy-MM-dd HH:mm");
            string formattedDate = Convert.ToDateTime(receipt.DATE_PURCHASE).ToString("yyyy-MM-dd");

            try
            {
                using (var command = new SqlCommand(@"INSERT INTO GOODS_RECEIPT (CODE, DATE_PURCHASE, DATE_REGISTRATION, TYPE_RECEIPT, TYPE_DOCUMENT, DOCUMENT_NUMBER, ANNOTATIONS, STATUS, PK_SUPPLIER, PK_USER, PK_WAREHOUSE) VALUES (@CODE, @DATE_PURCHASE, @DATE_REGISTRATION, @TYPE_RECEIPT, @TYPE_DOCUMENT, @DOCUMENT_NUMBER, @ANNOTATIONS, @STATUS, @PK_SUPPLIER, @PK_USER, @PK_WAREHOUSE);
                                                            SELECT SCOPE_IDENTITY();", connection, transaction))
                {
                    string code = await GenerateCode(connection, transaction, receipt.PK_WAREHOUSE);
                    command.Parameters.Add("@CODE", SqlDbType.NVarChar, 15).Value = code;
                    command.Parameters.Add("@DATE_PURCHASE", SqlDbType.Date).Value = formattedDate;
                    command.Parameters.Add("@DATE_REGISTRATION", SqlDbType.DateTime).Value = creationDate;
                    command.Parameters.Add("@TYPE_RECEIPT", SqlDbType.VarChar, 15).Value = receipt.TYPE_RECEIPT;
                    command.Parameters.Add("@TYPE_DOCUMENT", SqlDbType.VarChar, 15).Value = receipt.TYPE_DOCUMENT;
                    command.Parameters.Add("@DOCUMENT_NUMBER", SqlDbType.VarChar, 30).Value = receipt.DOCUMENT_NUMBER;
                    command.Parameters.Add("@ANNOTATIONS", SqlDbType.NVarChar, 80).Value = receipt.ANNOTATIONS;
                    command.Parameters.Add("@STATUS", SqlDbType.VarChar, 10).Value = "ACTIVO";
                    command.Parameters.Add("@PK_SUPPLIER", SqlDbType.Int).Value = receipt.PK_SUPPLIER;
                    command.Parameters.Add("@PK_USER", SqlDbType.Int).Value = receipt.PK_USER;
                    command.Parameters.Add("@PK_WAREHOUSE", SqlDbType.Int).Value = receipt.PK_WAREHOUSE;

                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting receipt.", ex);
            }
        }

        private async Task InsertDetail(SqlConnection connection, SqlTransaction transaction, GoodsReceipt receipt)
        {
            if (receipt?.details == null || !receipt.details.Any())
            {
                return;
            }

            try
            {
                int indexNumber = 1;

                foreach (var details in receipt.details)
                {
                    using (var command = new SqlCommand(@"INSERT INTO DETAILS_RECEIPT (PK_RECEIPT, PK_PRODUCT, QUANTITY, COST, INDEX_NUMBER) VALUES (@PK_RECEIPT, @PK_PRODUCT, @QUANTITY, @COST, @INDEX_NUMBER)", connection, transaction))
                    {
                        command.Parameters.Add("@PK_RECEIPT", SqlDbType.Int).Value = receipt.PK_RECEIPT;
                        command.Parameters.Add("@PK_PRODUCT", SqlDbType.Int).Value = details.PK_PRODUCT;
                        command.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = details.QUANTITY;
                        command.Parameters.Add("@COST", SqlDbType.Int).Value = details.COST;
                        command.Parameters.Add("@INDEX_NUMBER", SqlDbType.Int).Value = indexNumber;

                        await command.ExecuteNonQueryAsync();
                        await IncreaseQuantity(connection, transaction, receipt.PK_WAREHOUSE, details.PK_PRODUCT, details.QUANTITY);
                        indexNumber++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting details into the receipt.", ex);
            }
        }

        public async Task<IEnumerable<DetailsReceipt>> ReadDetails(int id)
        {
            var details = new List<DetailsReceipt>();
            try
            {
                using (var connection = _connectionData.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"SELECT PRODUCTS.CODE, PRODUCTS.DESCRIPTION, PRODUCTS.MATERIAL, PRODUCTS.COLOUR, CATEGORIES.CATEGORY_NAME, BRANDS.BRAND_NAME, DETAILS_RECEIPT.QUANTITY, DETAILS_RECEIPT.COST
                                                            FROM DETAILS_RECEIPT INNER JOIN PRODUCTS ON DETAILS_RECEIPT.PK_PRODUCT = PRODUCTS.PK_PRODUCT INNER JOIN BRANDS ON PRODUCTS.PK_BRAND = BRANDS.PK_BRAND INNER JOIN CATEGORIES ON PRODUCTS.PK_CATEGORY = CATEGORIES.PK_CATEGORY 
                                                            WHERE DETAILS_RECEIPT.PK_RECEIPT = @id ORDER BY DETAILS_RECEIPT.INDEX_NUMBER", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new DetailsReceipt
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
                                    COST = Convert.ToInt32(reader["COST"]),

                                });
                            }
                        }
                    }
                }
                return details;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading goods receipt details", ex);
            }
        }

        private async Task IncreaseQuantity(SqlConnection connection, SqlTransaction transaction, int PK_WAREHOUSE, int PK_PRODUCT, int QUANTITY)
        {
            try
            {
                using (var command = new SqlCommand(@"SELECT COUNT(*) FROM DETAILS_WAREHOUSE WHERE PK_PRODUCT = @PK_PRODUCT AND PK_WAREHOUSE = @PK_WAREHOUSE", connection, transaction))
                {
                    command.Parameters.Add("@PK_WAREHOUSE", SqlDbType.Int).Value = PK_WAREHOUSE;
                    command.Parameters.Add("@PK_PRODUCT", SqlDbType.Int).Value = PK_PRODUCT;

                    if (Convert.ToInt32(await command.ExecuteScalarAsync()) > 0)
                    {
                        command.CommandText = @"UPDATE DETAILS_WAREHOUSE SET STOCK = STOCK + @QUANTITY WHERE PK_PRODUCT = @PK_PRODUCT AND PK_WAREHOUSE = @PK_WAREHOUSE";
                        command.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = QUANTITY;
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO DETAILS_WAREHOUSE (PK_WAREHOUSE, PK_PRODUCT, STOCK, PRICE) VALUES (@PK_WAREHOUSE, @PK_PRODUCT, @STOCK, @PRICE)";
                        command.Parameters.Add("@STOCK", SqlDbType.Int).Value = QUANTITY;
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

        private async Task DecreaseQuantity(SqlConnection connection, SqlTransaction transaction, int PK_RECEIPT)
        {
            try
            {
                var updates = new List<(int PK_WAREHOUSE, int PK_PRODUCT, int QUANTITY)>();

                using (var command = new SqlCommand(@"SELECT GOODS_RECEIPT.PK_WAREHOUSE, DETAILS_RECEIPT.PK_PRODUCT, DETAILS_RECEIPT.QUANTITY 
                                                            FROM DETAILS_RECEIPT INNER JOIN GOODS_RECEIPT ON DETAILS_RECEIPT.PK_RECEIPT = GOODS_RECEIPT.PK_RECEIPT 
                                                            WHERE GOODS_RECEIPT.PK_RECEIPT = @PK_RECEIPT", connection, transaction))
                {
                    command.Parameters.Add("@PK_RECEIPT", SqlDbType.Int).Value = PK_RECEIPT;

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
                    using (var updateCommand = new SqlCommand(@"UPDATE DETAILS_WAREHOUSE SET STOCK = STOCK - @QUANTITY WHERE PK_PRODUCT = @PK_PRODUCT AND PK_WAREHOUSE = @PK_WAREHOUSE", connection, transaction))
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

        private async Task<string> GenerateCode(SqlConnection connection, SqlTransaction transaction, int PK_WAREHOUSE)
        {
            try
            {
                string code = "";

                    using (var command = new SqlCommand("SELECT COUNT(*) + 1 AS CODE FROM GOODS_RECEIPT WHERE PK_WAREHOUSE = @id", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@id", PK_WAREHOUSE);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                code = "ENT-" + reader["CODE"].ToString();
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
