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

		static private CancellationTokenSource cts = new CancellationTokenSource();
		static private CancellationToken ct = cts.Token;

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
                

				if (flag)
                {

					await Task.Run(async () =>
					{
						Message msg = Message.fromJson(data);
						Message responseMsg;
						string responseMsgJs;
						byte[] responseDate;
						if (msg.Text == "exit")
						{
							Console.WriteLine("Serv stop working!");
							responseMsg = new Message("Server", "Serv stop working!");
							responseMsgJs = responseMsg.toJson();
							responseDate = Encoding.UTF8.GetBytes(responseMsgJs);
							await udpClient.SendAsync(responseDate, ep);
							cts.Cancel();

							//flag = false;
							
						}
						else
						{
							responseMsg = new Message("Server", "Message accept on serv!");
							responseMsgJs = responseMsg.toJson();
							Console.WriteLine(msg.ToString());
							responseDate = Encoding.UTF8.GetBytes(responseMsgJs);

							await udpClient.SendAsync(responseDate, ep);
						}
					});


				}
                else
                {
                    return;
                }
            }
        }
    }
}
