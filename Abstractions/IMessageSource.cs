using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chat_CodeFirst.Abstractions
{
    public interface IMessageSource
    {
        void SendMessage(MessageUDP messageUDP, IPEndPoint endPoint);

        MessageUDP ReceiveMessage(ref IPEndPoint endPint);

    }
}
