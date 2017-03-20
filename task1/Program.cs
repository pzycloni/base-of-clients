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
            string name = null;
            string password = null;

			Console.WriteLine("Введите свое имя: ");
			name = Console.ReadLine();

			Console.WriteLine("Введите пароль: ");
			password = Console.ReadLine();

            PreferredClient client = new PreferredClient(name, password);

            client.PutMoney(150000);

            Product macbook = new Product("mackbook", 100000);
            client.Buy(macbook);

            Product airpods = new Product("airpods", 2000);
            client.Buy(airpods);

            client.ShowClient();

            client.ShowProducts();

            /*DB db = new DB();
            db.DropTable("Clients");
            db.DropTable("Products");
            db.Drop();*/
        }
    }
}
