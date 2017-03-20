using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace task1
{
    class DB
    {
		private List<string> errors = null;

        public DB()
        {
			errors = new List<string>();
		}

        private string BuildString(List<string> rows, string wrapper = "")
        {
            string output = null;
            int counter = 0;

            foreach (string row in rows)
            {
                output += wrapper + row + wrapper;
                counter++;

                if (counter < rows.Count)
                    output += ", "; 
            }

            return output;
        }

        public void Create()
        {

            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                connection.Open();

                string query = "CREATE DATABASE " + "mytest" + ";";

				try {
					using (var command = new SqlCommand(query, connection)) {
						command.ExecuteNonQuery();
					}
				} catch (Exception e) {
					errors.Add(e.Message);
					Console.WriteLine("БД mytest уже существует!");
				}
                
            }
        }

        public void Show(string table, string where = "1")
        {
            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                string query = null;

                if (where == "1")
                {
                    query = "USE mytest; SELECT * FROM " + table + ";";
                }
                else
                {
                    query = "USE mytest; SELECT * FROM " + table + " WHERE " + where + ";";
                } 

                connection.Open();

				try {
					using (var command = new SqlCommand(query, connection)) {
						using (var reader = command.ExecuteReader()) {
							while (reader.Read()) {
								for (int col = 0; col < reader.FieldCount; col++) {
									string result = reader.GetName(col) + ": " + reader[col];
									Console.WriteLine(result);
								}
							}
						}
					}
				} catch (Exception e) {
					errors.Add(e.Message);
					Console.WriteLine("Таблицы " + table + " не существует!");
				}
                
            }
        } 

        public void CreateTable(string table, List<string> rows)
        {
            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                connection.Open();

                string query = "USE " + "mytest" + "; " +
                        "CREATE TABLE " + table +
                        "(" + this.BuildString(rows) + ");";
				try {
					using (var command = new SqlCommand(query, connection)) {
						command.ExecuteNonQuery();
					}
				} catch (Exception e){
					errors.Add(e.Message);
					Console.WriteLine("Таблица " + table + " уже существует!");
				}
                
            }
        }

        public void Insert(string table, List<KeyValuePair<string, string>> rows)
        {
            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                connection.Open();

                List<string> columns = new List<string>();
                foreach (KeyValuePair<string, string>column in rows) {
                    columns.Add(column.Key);
                }
                string fields = this.BuildString(columns);

                List<string> values = new List<string>();
                foreach (KeyValuePair<string, string>val in rows) {
                    values.Add(val.Value);
                }
                string vals = this.BuildString(values, "'");

                string query = "USE mytest; INSERT INTO " + table + 
                                "(" + fields + ") VALUES (" + vals + ");";
				try {
					using (var command = new SqlCommand(query, connection)) {
						command.ExecuteNonQuery();
					}
				} catch (Exception e) {
					errors.Add(e.Message);
					Console.WriteLine("Таблицы " + table + " не существует!");
				}
                
            }
        }

        public void Update(string table, List<KeyValuePair<string, string>> rows, string where)
        {
            string set = null;
            int counter = 0;

            foreach (KeyValuePair<string, string> row in rows)
            {
                set += row.Key + "='" + row.Value + "'";
                counter++;

                if (counter < rows.Count)
                {
                    set += ",";
                }
            }

            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                string query = "USE mytest; UPDATE " + table + " SET " + set + " WHERE " + where + ";";
                
                connection.Open();

				try {
					using (var command = new SqlCommand(query, connection)) {
						command.ExecuteNonQuery();
					}
				} catch (Exception e) {
					errors.Add(e.Message);
					Console.WriteLine("Таблицы " + table + " не существует!");
				}
                
				
            }

        }

        public void Drop()
        {
            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                string query = "DROP DATABASE mytest;";
                connection.Open();
				try {
					using (var command = new SqlCommand(query, connection)) {
						command.ExecuteNonQuery();
					}
				} catch (Exception e) {
					errors.Add(e.Message);
					Console.WriteLine("БД mytest не существует!");
				}
                
            }
        }

        public void DropTable(string table)
        {
            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                string query = "USE mytest; DROP TABLE " + table + ";";
                connection.Open();

				try {
					using (var command = new SqlCommand(query, connection)) {
						command.ExecuteNonQuery();
					}
				} catch (Exception e) {
					errors.Add(e.Message);
					Console.WriteLine("Таблицы " + table + " не существует!");
				}
                
                connection.Close();
            }
        }

		public void ShowErrors() {
			int counter = 1;
			foreach (string error in errors) {
				Console.WriteLine(counter.ToString() + ". " + error);
				counter++;
			}
		}

        public List<KeyValuePair<string, string>> Select(string table, string where)
        {
            List<KeyValuePair<string, string>> output = new List<KeyValuePair<string, string>>();

            using (var connection = new SqlConnection(task1.Properties.Settings.Default.Database1ConnectionString))
            {
                string query = "USE mytest; SELECT * FROM " + table +
                                " WHERE " + where + ";";

                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int col = 0; col < reader.FieldCount; col++)
                            {
                                output.Insert(col, new KeyValuePair<string, string>(reader.GetName(col).ToString(), reader[col].ToString()));
                            }
                        }
                    }
                }
            }
            return output;
        }
    }
}
