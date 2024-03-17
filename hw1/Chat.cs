using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace hw1
{
	public class Chat
	{
		

		private UdpClient ucl;

		public Chat(string ip, int port) 
		{
			this.ucl = new UdpClient(ip, port);
			ucl.Connect(ip, port);
		}

		public void Server()
		{
			IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
			UdpClient ucl = new UdpClient(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента");

            while (true)
            {
				try
				{
					byte[] buffer = ucl.Receive(ref ep);
					string str = Encoding.UTF8.GetString(buffer);

					Message? somemessage = Message.fromJson(str);
					if (somemessage != null)
					{
						Console.WriteLine(somemessage);
					}
					else
					{
                        Console.WriteLine("Некорректное сообщение");
                    }
				
				}
				catch (Exception e)
				{
                    Console.WriteLine(e.Message);
                }

            }
        }

		public void Client()
		{
			IPEndPoint localEP = new IPEndPoint((IPAddress)IPAddress.Parse("127.0.0.1"), 12345);
			UdpClient ucl = new UdpClient();
			Console.WriteLine("Enter name: ");
			string name = Console.ReadLine();


			while (true)
			{
				try
				{
					Console.WriteLine("Enter msg: ");
					string msg = Console.ReadLine();
					if (String.IsNullOrEmpty(msg))
                    {
						break;
                    }
					Message? somemessage = new Message(name, msg);
					string jsMsg = somemessage.toJson();


                    byte[] buffer = Encoding.UTF8.GetBytes(jsMsg);
					ucl.Send(buffer, localEP);

				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

			}
		}


	}
}
