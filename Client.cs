using Chat_CodeFirst.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Chat_CodeFirst
{
    internal class Client
    {
        private readonly IMessageSource _messageSource;
        private readonly string _name;
        private readonly IPEndPoint _endPoint;

        public Client(IMessageSource messageSource, IPEndPoint endPoint, string name)
        {
            _messageSource = messageSource;
            _endPoint = endPoint;
            _name = name;
        }

        private void Registered()
        {
            var messageJSON = new MessageUDP()
            {
                Command = Command.Register,
                FromName = _name,
            };
            _messageSource.SendMessage(messageJSON, _endPoint);
        }

        public void ClientSendler()
        {
            while (true)
            {
                Console.WriteLine("Input UserName:");
                string toUser = Console.ReadLine();
                if (string.IsNullOrEmpty(toUser))
                    continue;

                Console.WriteLine("Input message:");
                string text = Console.ReadLine();

                var messageJSON = new MessageUDP()
                {
                    Text = text,
                    FromName = _name,
                    ToName = toUser
                };
                _messageSource.SendMessage(messageJSON, _endPoint);
            }
        }

        public void ClientListener()
        {
            Registered();
            IPEndPoint endPoint = new IPEndPoint(_endPoint.Address, _endPoint.Port);
            while (true)
            {
                MessageUDP message = _messageSource.ReceiveMessage(ref endPoint);
                Console.WriteLine(message.ToString());
            }
        }
    }
}
