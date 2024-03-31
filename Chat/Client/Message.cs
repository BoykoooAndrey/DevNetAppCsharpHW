using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;


namespace Client
{
    internal class Message
    {
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Text { get; set; }

        public DateTime DateTime { get; set; }
		public Message(string fromName, string toName, string text)
		{
			this.FromName = fromName;
			this.ToName = toName;
			this.Text = text;
			this.DateTime = DateTime.Now;
		}
		public Message() { }

        public string toJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public static Message? fromJson(string jsonData)
        {
            return JsonSerializer.Deserialize<Message>(jsonData);

        }
        public override string ToString()
        {
            return String.Format($"{this.DateTime.ToString()} - {this.FromName}:{this.Text}");
        }
    }
}
