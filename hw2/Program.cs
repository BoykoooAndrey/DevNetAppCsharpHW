

using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace hw2
{
	internal class Program
	{

		static async void PrintAsync(string message)
		{
			await Task.Delay(1000);     // имитация продолжительной работы
			Console.WriteLine(message);
		}

		static async Task  Main(string[] args) 
		{
			PrintAsync("Hello World");
			PrintAsync("Hello METANIT.COM");

			Console.WriteLine("Main End");
			await Task.Delay(3000); // ждем завершения задач


		}




	}
}
