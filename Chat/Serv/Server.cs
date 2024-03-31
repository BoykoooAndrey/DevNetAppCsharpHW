using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serv
{
	internal class Server
	{
		static Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();


		static private CancellationTokenSource cts = new CancellationTokenSource();
		static private CancellationToken ct = cts.Token;

		private static async Task SendMsg(string nameFrom, string nameTo, string text, UdpClient udpClient, IPEndPoint ep)
		{
			Message responseMsg = new Message(nameFrom, nameTo, text);
			string responseMsgJs = responseMsg.toJson();
			byte[] responseDate = Encoding.UTF8.GetBytes(responseMsgJs);
			await udpClient.SendAsync(responseDate, ep);


		}

		public static async Task AcceptMsg()
		{
			bool flag = true;
			IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
			UdpClient udpClient = new UdpClient(16874);
			Console.WriteLine("Сервер ожидает сообщение");
			while (!ct.IsCancellationRequested)
			{


				byte[] buffer = udpClient.Receive(ref ep);
				string data = Encoding.UTF8.GetString(buffer);


				await Task.Run(async () =>
				{
					Message msg = Message.fromJson(data);
					if (msg.Text == "exit")
					{
						Console.WriteLine("Serv stop working!");
						await SendMsg("Server", "Client", "Serv stop working!", udpClient, ep);
						cts.Cancel();
						return;

					}
					else
					{
						if (msg.ToName.ToLower() == "server")
						{
							if (msg.Text.ToLower() == "register")
							{
								if (clients.TryAdd(msg.FromName.ToLower(), ep))
								{
									await Console.Out.WriteLineAsync("User " + msg.FromName + " registred");
									await SendMsg("Server", msg.FromName, "Client registred", udpClient, ep);
								}
								else
								{
									await Console.Out.WriteLineAsync(msg.FromName + " already exists");

									await SendMsg("Server", msg.FromName, "Client already exists", udpClient, ep);
								}
							}
							else if (msg.Text.ToLower() == "delete")
							{
								clients.Remove(msg.FromName);
								await Console.Out.WriteLineAsync(msg.FromName + " deleted");

								await SendMsg("Server", msg.FromName, "Client deleted", udpClient, ep);
							}
							else if (msg.Text.ToLower() == "list")
							{
								StringBuilder stringBuilder = new StringBuilder();
								foreach (var client in clients)
								{
									stringBuilder.Append(client.Key + "\n");
								}
								await SendMsg("Server", msg.FromName, "\nСписок клиентов: \n" + stringBuilder.ToString(), udpClient, ep);

							}
							else
							{
								await Console.Out.WriteLineAsync(msg.ToString());
								await SendMsg("Server", "Client", "Unknow action", udpClient, ep);
							}
						}
						else if (msg.ToName.ToLower() == "all")
						{
							foreach (var client in clients)
							{
								msg.ToName = client.Key;
								await SendMsg(msg.FromName, client.Key, msg.Text, udpClient, client.Value);

							}
							await SendMsg("Server", "Client", "Messag send all users", udpClient, ep);

						}

						else
						{

							if (clients.TryGetValue(msg.ToName.ToLower(), out IPEndPoint? adressatEp))
							{
								await SendMsg(msg.FromName, msg.ToName, msg.Text, udpClient, adressatEp);
								await SendMsg("Server", "Client", "Messag send for " + msg.ToName, udpClient, ep);
							}
							else
							{
								await SendMsg("Server", "Client", "User not found", udpClient, ep);

							}
						}
					}
				});
			}
		}
	}
}
