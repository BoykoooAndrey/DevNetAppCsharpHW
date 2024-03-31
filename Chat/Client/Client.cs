using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
		private static IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 16874);
		private static UdpClient udpClient = new UdpClient(16678);

		public static async Task ClientSendler()
        {
			while (true)
			{
				Task t = new Task(async () =>
				{
					Console.WriteLine("Enter nameFrom:");
					string nameFrom = Console.ReadLine();
					Console.WriteLine("Enter nameTo:");
					string nameTo = Console.ReadLine();
					Console.Write("Введите соощение:");
					string text = Console.ReadLine();

					Message msg = new Message(nameFrom, nameTo, text);
					string responseMsgJs = msg.toJson();
					byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);

					await udpClient.SendAsync(responseData, ep);
					t = Task.CompletedTask;
					
				});
				t.Start();
				while(!t.IsCompleted)
				{
					
				}
			}
			
			
		}

		public static async Task ClientListner()
		{
			while (true)
			{
				var receiveResult = await udpClient.ReceiveAsync();
				byte[] answerData = receiveResult.Buffer;
				string answerMsgJs = Encoding.UTF8.GetString(answerData);
				Message answerMsg = Message.fromJson(answerMsgJs);
				Console.WriteLine(answerMsg.ToString());
			}
			
		}

		
        


    }
}
