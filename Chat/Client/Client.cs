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
        public static async Task SendMsg(string name)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 16874);
            UdpClient udpClient = new UdpClient();
            Console.Write("Введите соощение:");
            string text = Console.ReadLine();
            Message msg = new Message(name, text);
            string responseMsgJs = msg.toJson();
            byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);
            await udpClient.SendAsync(responseData, ep);
			var receiveResult = await udpClient.ReceiveAsync();
			byte[] answerData = receiveResult.Buffer;
			string answerMsgJs = Encoding.UTF8.GetString(answerData);
            Message answerMsg = Message.fromJson(answerMsgJs);
            Console.WriteLine(answerMsg.ToString());

        }
        


    }
}
