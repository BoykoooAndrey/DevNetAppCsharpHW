

using Npgsql;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace hw2
{
	internal class Program
	{

		

		static void  Main(string[] args) 
		{
			Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
				Console.WriteLine(r.Next(1, 3));
                
            }


            string connectionString = "Host=localhost;Username=postgres;Password=example;Database=Test";
			using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();
				string query = "SELECT users.id, users.name, messages.message FROM users JOIN messages ON users.id = messages.user_id";

				using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
				{
					using (NpgsqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							int userId = reader.GetInt32(0);
							string userName = reader.GetString(1);
							string message = reader.GetString(2);
                            Console.WriteLine($"User ID: {userId}, User Name: {userName}, Message: {message}");
                        }
					}
				}
			}


		}




	}
}
