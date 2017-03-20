using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBasesSample
{
    //Класс корзины
    class Cart
    {
        private List<Product> products;

        public Cart()
        {
            this.products = new List<Product>();
        }

        //Возвращает массив всех продуктов в корзине
        public Product[] GetProductsArray()
        {
            return this.products.ToArray();
        }

        //Возвращает список всех продуктов в корзине
        public List<Product> GetProductsList()
        {
            return this.products;
        }

        public Dictionary<int, int> GetProductUnqieList()
        {
            Dictionary<int, int> productList = new Dictionary<int, int>();
            for (int i = 0; i < products.Count; i++)
            {
                if (productList.ContainsKey(products[i].id))
                {
                    productList[products[i].id] += 1;
                }
                else
                {
                    productList.Add(products[i].id,1);
                }
            }
            return productList;
        }

        //Добавляет продукт в корзину
        public void AddProduct(Product product)
        {
            this.products.Add(product);
        }

        //Рассчитывает и возвращает цену всех продуктов в корзине
        public int GetTotalPrice()
        {
            Product[] products = this.GetProductsArray();
            int totalPrice = 0;
            for (int i = 0; i < products.Length; i++)
            {
                totalPrice += products[i].cost;
            }
            return totalPrice;
        }

        //Очищает корзину
        public void deleteAllProducts()
        {
            this.products.Clear();
        }

        //Удаляет из корзины заданный товар
        public void deleteSingleProduct(Product product)
        {
            this.products.Remove(product);
        }

        //Сохраняет заказ в БД
        public bool SaveOrderInDB(int client_id)
        {
            Dictionary<int, int> order = GetProductUnqieList();
            foreach(KeyValuePair<int, int> item in order)
            {
                if(!TransactionModel.SaveTransaction(client_id, item.Key, item.Value))
                    return false;
            }
            return true;
        }
    }
}
