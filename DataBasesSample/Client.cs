using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBasesSample
{
    //Класс клиента интернет-магазина
    class Client : Person
    {
        protected int id;
        protected string email;
        protected string telephone;
        protected double balance;
        public Cart cart { get; private set;}

        public Client(string name, int age = 0, string email = "", string telephone = "", double balance = 0, Sex sex = Sex.Undefined,int id = 0)
        {
            this.id = id;
            this.name = name;
            if (age > 0){this.age = age;} 
            else {throw new System.ArgumentException("Возраст не может быть отрицательным числом", "age");}
            this.sex = sex;
            this.email = email;
            this.telephone = telephone;
            this.balance = Math.Round(balance,2);
            this.cart = new Cart();
        }

        //Возвращает E-mail клиента
        public string GetEmail()
        {
            return this.email;
        }

        //Возвращает телефон клиента
        public string GetTelephone()
        {
            return this.telephone;
        }

        //Возвращает текущий баланс клиента
        public double GetBalance()
        {
            return this.balance; 
        }

        //Устанавливает телефон клиента
        public void SetTelephone(string telephone)
        {
            this.telephone = telephone;
        }

        //Устанавливает телефон клиента
        public void SetEmail(string email)
        {
            this.email = email;
        }

        //Изменяет баланс на заданную сумму
        public void ChangeBalance(int money)
        {
            this.balance += money;
        }

        //Оформляет заказ, совершает покупку
        public bool Checkout()
        {
            int costAllProducts = this.cart.GetTotalPrice();
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

        public virtual void SaveClientInDB() 
        {
            if (ClientModel.ClientExist(this.id))
            {
                ClientModel.UpdateClient(this.id, this.name, this.age, this.sex, this.telephone, this.email, this.balance);
            }
            else
            {
                this.id = ClientModel.InsertClient(this.name, this.age, this.sex, this.telephone, this.email, this.balance);
            }
        }

        public string GetSex(Sex sex)
        {
            switch (sex)
            {
                case Sex.Man:
                    return "Man";
                case Sex.Woman:
                    return "Woman";
                case Sex.Undefined:
                    return "Undefined";
                default:
                    return "Undefined";
            }
        }

        public void PrintHistory()
        {
            Dictionary<Product,int> history = new Dictionary<Product,int>();
            history = TransactionModel.GetPurchaseHistory(this.id);
            foreach (KeyValuePair<Product, int> item in history)
            {
                Console.WriteLine(String.Format("Товар: {0}, Кол-во: {1} \n",item.Key.name,item.Value));
            }
        }

        //Определяет данные для вывода в консоль
        public override string ToString()
        {
            return string.Format(
                " ID: {0} \n Name: {1} \n Sex: {2} \n Telephone: {3} \n Email: {4} \n Balance: {5} \n",
                this.id, this.name, this.GetSex(this.sex), this.telephone,this.email,this.balance
                );
        }
    }
}
