using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;

namespace DataBasesSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //PreferredClient client = new PreferredClient("Андрей", 21, "raftor74@mail.ru", "", 50000, Sex.Man);
            //client.SaveClientInDB();
            //Product product = new Product("Ipod",2000);
            //product.SaveProductInDB();
            //client.cart.AddProduct(product);
            //client.CheckoutWithDebit();
            //client.PrintHistory();
            List<PreferredClient> clients = ClientModel.GetClientsList();
            for (int i = 0; i < clients.Count; i++)
            {
                Console.WriteLine(clients[i]);
                clients[i].PrintHistory();
            }
            
        }
    }
}
