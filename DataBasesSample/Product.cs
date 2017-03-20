using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBasesSample
{
    //Класс продукта
    class Product
    {
        public int id { get; set; }
        public string name { get; set;}
        public int cost { get; set; }
        public Product(string name, int cost,int id=0)
        {
            this.id = id;
            this.name = name;
            this.cost = cost;
        }

        public void SaveProductInDB()
        {
            if (ProductModel.ProductExist(this.id))
            {
                ProductModel.UpdateProduct(this.id, this.name, this.cost);
            }
            else
            {
                this.id = ProductModel.InsertProduct(this.name, this.cost);
            }
        }
    }
}
