namespace Client
{
	internal class Program
	{
		

		static async Task Main(string[] args)
		{
			Task t = Client.ClientListner();
			Task t2 = Client.ClientSendler();
			t.Start();
			t2.Start();


		}
	}
}
