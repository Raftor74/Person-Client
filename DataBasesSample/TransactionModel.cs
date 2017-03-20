using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataBasesSample
{
    class TransactionModel : Model
    {
        private static string tableOriginName = "Transactions";

        public static Dictionary<Product,int> GetPurchaseHistory(int clientId)
        {
            Dictionary<Product, int> history = new Dictionary<Product, int>();
            if (!ClientModel.ClientExist(clientId))
            {
                Console.WriteLine("Пользователь с таким ID несуществует");
                return history;
            }

            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "SELECT product_id, count FROM " + tableOriginName +" WHERE client_id = @id;";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("id", clientId));
                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product readProduct = ProductModel.GetProductAsObject(int.Parse(reader[0].ToString()));
                            int count = int.Parse(reader[1].ToString());
                            if(readProduct != null)
                                history.Add(readProduct,count);
                        }
                    }
                    conn.Close();
                    return history;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return history;
                }
            }
        }

        public static bool SaveTransaction(int clientId, int productId, int count)
        {
            if(!(ClientModel.ClientExist(clientId) && ProductModel.ProductExist(productId)))
            {
                Console.WriteLine("Клиент или Товар с данным ID несуществуют. Ошибка сохранения транзакции");
                return false;
            }
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "INSERT INTO " + tableOriginName + " VALUES(@client_id,@product_id,@count);";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("client_id", clientId));
                command.Parameters.Add(new SqlParameter("product_id", productId));
                command.Parameters.Add(new SqlParameter("count", count));
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }
    }
}
