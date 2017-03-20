using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    class PreferredClient : Client
    {
		private string secret_word = null;

        public PreferredClient(string name, string password) :
            base (name, password)
        {
			secret_word = "DX";

			// создаем таблицы, если их нет
			CreateDB();
			CreateClientTable();
			CreateProductTable();

			// загрузка данных из бд
			if (Exist(name)) {
				LoadInformation();
				//Console.WriteLine(money);
				double sum = SumProducts();
				// установкливаем скидку
				if (SumProducts() >= 10000) {
					discount = 0.1;
					PutDiscount(discount);
				}

			} else {

				// Записываем нового пользователя в бд
				Console.WriteLine("Введите свое имя: ");
				name = Console.ReadLine();
				Console.WriteLine("Введите свой возраст: ");
				age = Convert.ToInt32(Console.ReadLine());
				Console.WriteLine("Введите свой пол(man, woman): ");
				sex = Console.ReadLine();
				Console.WriteLine("Введите свой email: ");
				string email = Console.ReadLine();
				Console.WriteLine("Введите пароль: ");
				password = Console.ReadLine();

				Register(name, age, sex, email, password);

				Console.WriteLine("Хотите положить деньги на счет(Y/N): ");
				string answer = Console.ReadLine();
				
				// кладем деньги
				if ("y".ToUpper() == answer || "y" == answer) {
					Console.WriteLine("Хотите положить деньги на счет(Y/N): ");
					double cash = Convert.ToDouble(Console.ReadLine());
					PutMoney(cash);
				}
				
			}
        }

		public void SetSecretWord(string word) {
			if (word != secret_word) {
				Console.WriteLine("Неверное кодовое слово!");
				return;
			}

			discount = 0.1;
			PutDiscount(discount);
		}
    }
}
