using Serv.Models;
using System.Text.Json;

public enum Command
{
	Register,
	Message,
	Confirmation
}

public class MessageUDP
{
	public int? Id { get; set; }
	public Command Command { get; set; }
	public string Text { get; set; }

	public string FromName { get; set; }
	public string ToName { get; set; }
	public bool Received { get; set; }

	public User? FromUser { get; set; }
	public User? ToUser { get; set; }
	public DateTime DateTime { get; set; }


    public MessageUDP()
    {
			this.DateTime = DateTime.Now;

	}
	// Метод для сериализации в JSON
	public string ToJson()
	{
		return JsonSerializer.Serialize(this);
	}

	// Статический метод для десериализации JSON в объект MyMessage
	public static MessageUDP FromJson(string json)
	{
		return JsonSerializer.Deserialize<MessageUDP>(json);
	}

	public override string ToString()
	{
		return String.Format($"{this.DateTime.ToString()} - {this.FromName}:{this.Text}");
	}
}