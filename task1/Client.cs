using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace task1
{
    class Client : Person
    {
        protected DB db;
        protected List<string> columns;

		protected double discount;
        private string email;
        private string password;
        protected double money;


        public Client(string name, string password)
        {
            this.name = name;
            this.password = password;

			this.discount = 0;

            db = new DB();

            this.columns = new List<string>() {
                "Name",
                "Age",
                "Sex",
                "Email",
                "Password",
                "Money",
				"Discount"
            };
       
        }

		public bool Exist(string name) {
			int count = 0;

			count = db.Select("Clients", "Name='" + name + "'").Count;
			return Convert.ToBoolean(count);
		}

        public void LoadInformation() {
            List<KeyValuePair<string, string>> information = db.Select("Clients", "Name='" + name + "'");

            // присвоение загруженных параметров
            foreach (KeyValuePair<string, string> info in information)
            {
                switch (info.Key)
                {
                    case "Name":
                        this.name = info.Value;
                        break;

                    case "Age":
                        this.age = Convert.ToInt32(info.Value);
                        break;

                    case "Sex":
                        this.sex = info.Value;
                        break;

                    case "Email":
                        this.email = info.Value;
                        break;

                    case "Password":
                        this.password = info.Value;
                        break;

                    case "Money":
                        this.money = Convert.ToDouble(info.Value);
                        break;

					case "Discount":
						this.discount = Convert.ToDouble(info.Value);
						break;
                }
            }
        }

        public bool PutMoney(double money = 0)
        {
            if (money > 0)
            {
                this.money += money;

                List<KeyValuePair<string, string>> set = new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("Money", this.money.ToString())
                };

                db.Update("Clients", set, "Name='" + this.name + "'");
                return true;
            }
            return false;
        }

		public void PutDiscount(double discount) {
			string where = "Name='" + this.name + "'";
			db.Update("Clients", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(columns[6], discount.ToString()) }, where);
		}

        public bool Buy(Product product)
        {
            List<KeyValuePair<string, string>> product_parameters = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("Customer", this.name),
                new KeyValuePair<string, string>("Name", product.name),
                new KeyValuePair<string, string>("Price", product.price.ToString())
            };
			
            if (product.price <= this.money)
            {
				this.money -= product.price;
				product.price -= product.price * this.discount;

				List<KeyValuePair<string, string>> client_parameters = new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("Money", this.money.ToString())
				};
				
                db.Insert("Products", product_parameters);
				db.Update("Clients", client_parameters, "Name='" + this.name + "'");
                Console.WriteLine("Покупка совершена!");

                return true;
            }

            Console.WriteLine("Недостаточно средств на вашем счету! Пополните счет и попробуйте снова!");
           
            return false;
        }

		public double SumProducts() {
			List<KeyValuePair<string, string>> rows = db.Select("Products", "Customer='" + this.name + "'");
			double sum = 0;

			foreach (KeyValuePair<string, string> row in rows) {
				if (row.Key == "Price") {
					sum += Convert.ToDouble(row.Value);
				}
			}
			return sum;
		}

        protected void Register(string name, int age, string sex, string email, string password, double cash = 0f)
        {
			if (this.Exist(name)) {
				Console.WriteLine("Пользователь с таким именем уже существует!");
				return;
			}

            // поля и значения
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>(this.columns[0], name),
                new KeyValuePair<string, string>(this.columns[1], age.ToString()),
                new KeyValuePair<string, string>(this.columns[2], sex),
                new KeyValuePair<string, string>(this.columns[3], email),
                new KeyValuePair<string, string>(this.columns[4], password),
                new KeyValuePair<string, string>(this.columns[5], cash.ToString()),
				new KeyValuePair<string, string>(this.columns[6], discount.ToString())
            };

			db.Insert("Clients", parameters);
            
        }

        public void ShowClient()
        {
            db.Show("Clients", "Name='" + this.name + "'");
        }

        public void ShowProducts()
        {
            db.Show("Products", "Customer='" + this.name + "'");
        }

        protected void CreateClientTable()
        {
            // параметры для создания таблицы
            List<string> options = new List<string>() {
                "ID INT IDENTITY(1,1) NOT NULL",
                "Name NVARCHAR(100) NULL",
                "Age NVARCHAR(10) NULL",
                "Sex NVARCHAR(10) NULL",
                "Email NVARCHAR(100) NULL",
                "Password NVARCHAR(100) NULL",
                "Discount NVARCHAR(100) NULL",
                "Money NVARCHAR(100) NULL",
                "CONSTRAINT PK_Client PRIMARY KEY (ID ASC)"
            };
            // создание таблицы
            db.CreateTable("Clients", options);
        }

        protected void CreateProductTable()
        {
            // параметры для создания таблицы
            List<string> options = new List<string>() {
                "ID INT IDENTITY(1,1) NOT NULL",
                "Customer NVARCHAR(100) NOT NULL",
                "Name NVARCHAR(100) NULL",
                "Price NVARCHAR(10) NULL",
                "CONSTRAINT PK_Product PRIMARY KEY (ID ASC)"
            };
            // создание таблицы
            db.CreateTable("Products", options);
        }

		protected void CreateDB() {
			db.Create();
		}
    }
}
