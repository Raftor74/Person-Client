using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBasesSample
{
    //Привелегилированный клиент со скидкой
    class PreferredClient : Client
    {
        private double debit;
        public PreferredClient(string name = null, int age = 0, string email = "", string telephone = "", double balance = 0, Sex sex = Sex.Undefined,int id = 0, double debit = 0.1) : base(name,age,email,telephone,balance,sex,id)
        {
            if (Math.Abs(debit) > 0.5)
            {
                Console.WriteLine("Невозможно установить скидку более 50%");
                return;
            }
            this.debit = Math.Abs(debit);
        }

        //Cовершает покупку с бонусами
        public bool CheckoutWithDebit() 
        {
            int costAllProducts = this.cart.GetTotalPrice();
            costAllProducts = (int)Math.Round(costAllProducts - costAllProducts * this.debit);
            double clientBalance = this.balance;
            if (costAllProducts > clientBalance)
            {
                double difference = Math.Abs(costAllProducts - clientBalance);
                Console.WriteLine(String.Format("Невозможно совершить покупку, недостаточно средств на вашем счету. Необходимо ещё {0} для совершения покупки.", difference));
                return false;
            }
            else
            {
                if (this.cart.SaveOrderInDB(this.id))
                {
                    this.balance = clientBalance - costAllProducts;
                    this.cart.deleteAllProducts();
                    SaveClientInDB();
                    Console.WriteLine("Покупка успешно совершена!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Не удалось совершить покупку!");
                    return false;
                }
            }
        }

        public override void SaveClientInDB()
        {
            if (ClientModel.ClientExist(this.id))
            {
                ClientModel.UpdateClient(this.id, this.name, this.age, this.sex, this.telephone, this.email, this.balance,this.debit);
            }
            else
            {
                this.id = ClientModel.InsertClient(this.name, this.age, this.sex, this.telephone, this.email, this.balance, this.debit);
            }
        }
    }
}
