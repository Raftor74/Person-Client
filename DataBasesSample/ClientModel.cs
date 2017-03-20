using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataBasesSample
{

    class ClientModel : Model
    {
        private static string tableName = "Clients";

        //Выводит на экран данные всех клиентов
        public static void PrintAllClients()
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

        //Выводит на экран данные указанного клиента
        public static void PrintClient(int id)
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "SELECT * FROM " + tableName + " WHERE id = @id;";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("id",id));
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

        //Возвращает список всех клиентов
        public static List<PreferredClient> GetClientsList()
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                List<PreferredClient> clients = new List<PreferredClient>();
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
                            int clientId = 0;
                            string clientName = "";
                            int clientAge = 0;
                            string clientEmail = "";
                            string clientTelephone = "";
                            double clientBalance = 0;
                            double clientDebit = 0;
                            Sex clientSex = Sex.Undefined;
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                switch (reader.GetName(column))
                                {
                                    case "Id":
                                        clientId = int.Parse(reader[column].ToString());
                                        break;
                                    case "name":
                                        clientName = reader[column].ToString();
                                        break;
                                    case "age":
                                        clientAge = int.Parse(reader[column].ToString());
                                        break;
                                    case "email":
                                        clientEmail = reader[column].ToString();
                                        break;
                                    case "telephone":
                                        clientTelephone = reader[column].ToString();
                                        break;
                                    case "balance":
                                        clientBalance = double.Parse(reader[column].ToString());
                                        break;
                                    case "debit":
                                        clientDebit = double.Parse(reader[column].ToString());
                                        break;
                                    case "sex":
                                        clientSex = getSexFromCode(int.Parse(reader[column].ToString()));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            PreferredClient client = new PreferredClient(clientName, clientAge, clientEmail, clientTelephone, clientBalance, clientSex, clientId, clientDebit);
                            clients.Add(client);
                        }
                        conn.Close();
                        return clients;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        //Возвращает определённого клиента
        public static PreferredClient GetClientAsObject(int id)
        {
            if (!ClientExist(id))
            {
                Console.WriteLine("Пользователь с таким ID не найден. Ошибка получения данных");
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
                        int clientId = 0;
                        string clientName = "";
                        int clientAge = 0;
                        string clientEmail = "";
                        string clientTelephone = "";
                        double clientBalance = 0;
                        double clientDebit = 0;
                        Sex clientSex = Sex.Undefined;
                        while (reader.Read())
                        {
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                switch (reader.GetName(column))
                                {
                                    case "Id":
                                        clientId = int.Parse(reader[column].ToString());
                                        break;
                                    case "name":
                                        clientName = reader[column].ToString();
                                        break;
                                    case "age":
                                        clientAge = int.Parse(reader[column].ToString());
                                        break;
                                    case "email":
                                        clientEmail = reader[column].ToString();
                                        break;
                                    case "telephone":
                                        clientTelephone = reader[column].ToString();
                                        break;
                                    case "balance":
                                        clientBalance = double.Parse(reader[column].ToString());
                                        break;
                                    case "debit":
                                        clientDebit = double.Parse(reader[column].ToString());
                                        break;
                                    case "sex":
                                        clientSex = getSexFromCode(int.Parse(reader[column].ToString()));
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        PreferredClient client = new PreferredClient(clientName, clientAge, clientEmail, clientTelephone, clientBalance, clientSex, clientId, clientDebit);
                        conn.Close();
                        return client;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        //Проверяет, существует ли клиент с данным id
        public static bool ClientExist(int id)
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

        //Получает код Пола для сохранения в БД
        public static int getSexCode(Sex sex)
        {
            switch (sex)
            {
                case Sex.Man:
                    return 1;
                case Sex.Woman:
                    return 2;
                case Sex.Undefined:
                    return 0;
                default:
                    return 0;
            }
        }

        //Получает Пол по коду из БД
        public static Sex getSexFromCode(int sex)
        {
            switch (sex)
            {
                case 1:
                    return Sex.Man;
                case 2:
                    return Sex.Woman;
                case 0:
                    return Sex.Undefined;
                default:
                    return Sex.Undefined;
            }
        }

        //Вставляет новую запись клиента. Возвращает ID только что добавленного товара
        public static int InsertClient(string name = "", int age = 0, Sex sex = Sex.Undefined, string telephone = "", string email = "", double balance = 0, double debit = 0)
        {
            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "INSERT INTO " + tableName + " VALUES(@name,@age,@email,@telephone,@balance,@debit,@sex)";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("name", name));
                command.Parameters.Add(new SqlParameter("age", age));
                command.Parameters.Add(new SqlParameter("email", email));
                command.Parameters.Add(new SqlParameter("telephone", telephone));
                command.Parameters.Add(new SqlParameter("balance", balance));
                command.Parameters.Add(new SqlParameter("debit", debit));
                command.Parameters.Add(new SqlParameter("sex", getSexCode(sex)));
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

        //Обновляет данные о клиенте
        public static void UpdateClient(int id, string name = "", int age = 0, Sex sex = Sex.Undefined, string telephone = "", string email = "", double balance = 0, double debit = 0)
        {
            if (!ClientExist(id))
            {
                Console.WriteLine("Пользователь с таким ID несуществует. Ошибка обновления данных");
                return;
            }

            using (var conn = new SqlConnection(dbConnStr))
            {
                string sql = "UPDATE " + tableName + " SET name = @name,age = @age, email = @email,telephone = @telephone, balance = @balance,debit = @debit, sex = @sex WHERE id = @id";
                sql = prepareSqlCommand(sql);
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("name", name));
                command.Parameters.Add(new SqlParameter("age", age));
                command.Parameters.Add(new SqlParameter("email", email));
                command.Parameters.Add(new SqlParameter("telephone", telephone));
                command.Parameters.Add(new SqlParameter("balance", balance));
                command.Parameters.Add(new SqlParameter("debit", debit));
                command.Parameters.Add(new SqlParameter("sex", getSexCode(sex)));
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
