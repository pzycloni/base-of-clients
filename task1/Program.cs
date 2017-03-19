using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Alex";
            string sex = "man";
            int age = 22;
            string email = "gukklucky@gmail.com";
            string password = "qwertyui";

            //DB db = new DB();
            //db.Create();

            PreferredClient client = new PreferredClient(name, password);

            //client.CreateClientTable();
            //client.CreateProductTable();

            //записываем клиента в бд
            //client.Register(name, age, sex, email, password);

            client.LoadInformation();
            
            client.PutMoney(150000);

            Product macbook = new Product("mackbook", 100000);
            //client.Buy(macbook);

            Product airpods = new Product("airpods", 2000);
            //client.Buy(airpods);

            string users = "Clients",
                   products = "Products";

            client.ShowClient();

            client.ShowProducts();

            // записываем клиента в бд
            //client.Register(name, age, sex, email, password);
            // вывести всех клиентов
            //client.ShowClient();
            

            //db.DropTable(users);
            //db.DropTable(products);
            //db.Drop();
        }
    }
}
