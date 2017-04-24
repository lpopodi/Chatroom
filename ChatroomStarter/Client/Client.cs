using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public string displayName;
        public Client(string IP, int port)
        {
            //Console.WriteLine("Enter your display name");
            //string displayName = Console.ReadLine();
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Send()
        {
            while (true)
            {
                string messageString = UI.GetInput();
                byte[] message = Encoding.ASCII.GetBytes(messageString);
                stream.Write(message, 0, message.Count());
            }
            
        }
        public void Recieve()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
            }
            
        }

        public void SetDisplayName()
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

        private void Send(string v)
        {
            
        }
    }
}
