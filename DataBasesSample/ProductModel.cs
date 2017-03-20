using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataBasesSample
{
    class ProductModel : Model
    {
        private static string tableName = "Products";

        //Выводит на экран данные всех клиентов
        public static void PrintAllProducts()
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "SELECT * FROM " + tableName + ";";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                for (int column = 0; column < reader.FieldCount; column++)
                                {
                                    Console.WriteLine(reader.GetName(column) + "=" + reader[column]);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("NO DATA");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Выводит на экран данные указанного товара
        public static void PrintProduct(int id)
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "SELECT * FROM " + tableName + " WHERE id = @id;";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("id", id));
                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                for (int column = 0; column < reader.FieldCount; column++)
                                {
                                    Console.WriteLine(reader.GetName(column) + "=" + reader[column]);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("NO DATA");
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Возвращает список всех товаров
        public static List<Product> GetProductList()
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                List<Product> products = new List<Product>();
                string sql = "SELECT * FROM " + tableName + ";";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = 0;
                            string productName = "";
                            int productCost = 0;
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                switch (reader.GetName(column))
                                {
                                    case "Id":
                                        productId = int.Parse(reader[column].ToString());
                                        break;
                                    case "name":
                                        productName = reader[column].ToString();
                                        break;
                                    case "cost":
                                        productCost = int.Parse(reader[column].ToString());
                                        break;
                                    default:
                                        break;
                                }
                            }
                            Product product = new Product(productName, productCost, productId);
                            products.Add(product);
                        }
                        conn.Close();
                        return products;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        //Возвращает определённый товар
        public static Product GetProductAsObject(int id)
        {
            if (!ProductExist(id))
            {
                Console.WriteLine("Товар с таким ID не найден. Ошибка получения данных");
                return null;
            }
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "SELECT * FROM " + tableName + " WHERE id = @id;";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("id", id));
                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        int productId = 0;
                        string productName = "";
                        int productCost = 0;
                        while (reader.Read())
                        {
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                switch (reader.GetName(column))
                                {
                                    case "Id":
                                        productId = int.Parse(reader[column].ToString());
                                        break;
                                    case "name":
                                        productName = reader[column].ToString();
                                        break;
                                    case "cost":
                                        productCost = int.Parse(reader[column].ToString());
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        Product product = new Product(productName, productCost, productId);
                        conn.Close();
                        return product;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        //Проверяет, существует ли товар с данным id
        public static bool ProductExist(int id)
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "SELECT * FROM " + tableName + " WHERE id = @id;";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("id", id));
                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            conn.Close();
                            return true;
                        }
                        else
                        {
                            conn.Close();
                            return false;
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        
        //Вставляет новую запись товара. Возвращает ID только что добавленного товара
        public static int InsertProduct(string name = "", int cost = 0)
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "INSERT INTO " + tableName + " VALUES(@name,@cost)";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("name", name));
                command.Parameters.Add(new SqlParameter("cost", cost));
                int lastId = 0;
                try
                {
                    conn.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT @@IDENTITY";
                        lastId = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0;
                    }
                    conn.Close();
                    return lastId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        //Обновляет данные о товаре
        public static void UpdateProduct(int id, string name = "", int cost = 0)
        {
            if (!ProductExist(id))
            {
                Console.WriteLine("Товар с таким ID несуществует. Ошибка обновления данных");
                return;
            }

            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "UPDATE " + tableName + " SET name = @name,cost = @cost, email = @email,telephone = @telephone, balance = @balance,debit = @debit, sex = @sex WHERE id = @id";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("name", name));
                command.Parameters.Add(new SqlParameter("cost", cost));
                command.Parameters.Add(new SqlParameter("id", id));
                try
                {
                    conn.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
