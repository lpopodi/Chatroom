using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        private string displayName;

        public string DisplayName { get { return displayName; } set { displayName = value; } }

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = Guid.NewGuid().ToString();
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            while (true)
            {
                try
                {
                    byte[] recievedMessage = new byte[256];
                    stream.Read(recievedMessage, 0, recievedMessage.Length);
                    string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
                    Message message = new Message(null, recievedMessageString);
                    Server.messageQueue.Enqueue(message);
                    Console.WriteLine(recievedMessageString);
                }
                catch
                {
                    Console.WriteLine("Something has gone wrong :( ");
                }
            }  
        }

        public void GetDisplayName()
        {
            Send("Enter your display name");
            byte[] recievedMessage = new byte[56];
            try
            {
                stream.Read(recievedMessage, 0, recievedMessage.Length);
            }
            catch
            {
                Console.WriteLine("Error trying to set DisplayName for chat");
            }
        }




    }
}
