using Chat_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chat_CodeFirst
{
    public enum Command
    {
        Register,
        Message,
        Confirmation
    }
    public class MessageUDP
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string? Text { get; set; }
        public string? ToName { get; set; }
        public string? FromName { get; set; }

        public string ToJSON()
        {
            return JsonSerializer.Serialize(this);
        }

        public static MessageUDP FromJSON(string json)
        {
            return JsonSerializer.Deserialize<MessageUDP>(json);
        }
    }
}
