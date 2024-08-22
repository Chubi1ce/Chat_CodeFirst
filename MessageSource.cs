using Chat_CodeFirst.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat_CodeFirst
{
    internal class MessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;
        public MessageSource(int port)
        {
            _udpClient = new UdpClient(port);
        }

        public MessageUDP ReceiveMessage(ref IPEndPoint endPint)
        {
            byte[] bytes = _udpClient.Receive(ref endPint);
            string message = Encoding.UTF8.GetString(bytes);
            return MessageUDP.FromJSON(message);
        }

        public void SendMessage(MessageUDP messageUDP, IPEndPoint endPoint)
        {
            string json = messageUDP.ToJSON();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            _udpClient.Send(bytes, bytes.Length, endPoint);
        }
    }
}
