using Chat_CodeFirst.Abstractions;
using Chat_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Chat_CodeFirst
{
    public class Server
    {
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        UdpClient udpClient;
        private readonly IMessageSource _messageSource;
        private readonly IPEndPoint _udpEndPoint;

        public Server(IMessageSource messageSource, IPEndPoint udpEndPoint)
        {
            _messageSource = messageSource;
            _udpEndPoint = udpEndPoint;
        }

        public void Register(MessageUDP message, IPEndPoint fromEndPoint)
        {
            Console.WriteLine($"Registering user: {message.FromName}");
            clients.Add(message.FromName, fromEndPoint);

            using (var chatConext = new ChatContext())
            {
                if (chatConext.Users.FirstOrDefault(x => x.Name == message.FromName) != null)
                    return;
                chatConext.Add(new User { Name = message.FromName });
                chatConext.SaveChanges();
            }
        }

        public void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);
            using (var chatConext = new ChatContext())
            {
                var msg = chatConext.Messages.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.Received = true;
                    chatConext.SaveChanges();
                }
            }
        }

        public void RelyMessage(MessageUDP message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out IPEndPoint ep))
            {
                using (var chatConext = new ChatContext())
                {
                    var fromUser = chatConext.Users.First(x => x.Name == message.FromName);
                    var toUser = chatConext.Users.First(x => x.Name == message.FromName);
                    var msg = new Message
                    {
                        FromUser = fromUser,
                        ToUser = toUser,
                        Received = false,
                        Text = message.Text
                    };
                    chatConext.Messages.Add(msg);
                    chatConext.SaveChanges();
                    id = msg.Id;
                }
                var forwardMessageJson = new MessageUDP()
                {
                    Id = id,
                    Command = Command.Message,
                    ToName = message.ToName,
                    FromName = message.FromName,
                    Text = message.Text
                };
                //byte[] forwardBytes = Encoding.UTF8.GetBytes(forwardMessageJson);
                _messageSource.SendMessage(forwardMessageJson,_udpEndPoint);
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        void ProcessMessage(MessageUDP message, IPEndPoint fromEndPoint)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой { message.Command}:");
            Console.WriteLine(message.Text);
            if (message.Command == Command.Register)
            {
                Register(message, new IPEndPoint(fromEndPoint.Address, fromEndPoint.Port));
            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }
        }
    }
}
