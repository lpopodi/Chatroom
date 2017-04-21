using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        public static Client client;
        public static Dictionary<string, Client> _activeChatClients = new Dictionary<string, Client>();
        TcpListener server;

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.106"), 6915);
            server.Start();
            
        }
        public void Run()
        {
            Console.WriteLine("Chat Server Started");

            Parallel.Invoke(AcceptClient, Respond);
            
                      
        }
        private void AcceptClient()
        {
            try
            {
                Console.WriteLine("Waiting for incoming client connections");
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("New Client Connected");
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket);
                //Thread chatThread = new Thread(new ParameterizedThreadStart(StartChat));
                //chatThread.Start(client);
                Thread chatThread = new Thread(() => StartChat(client));
                chatThread.Start();
                Console.WriteLine($"==> {client.displayName} ");
                //_activeChatClients.Add(client.UserId, client);
            }
                catch (Exception)
                {
                    Console.WriteLine("An error occurred while trying to create a threaded client");
                }

}

        private void StartChat(Client client)
        {
            //adding client to dictionary
            _activeChatClients.Add(client.UserId, client);
            Thread thread = new Thread(new ThreadStart(client.Recieve));
            thread.Start();
            //Task.Run(() => { client.Recieve(); });
            //Task.Run(() => { Respond(); });

        }

        private void Respond()
        {
            while (true)
            {
                Message message = default(Message);
                if (messageQueue.TryDequeue(out message))
                {
                    client.Send(message.Body);
                }
            }
        }

        private void NotifyNewUser(string displayName)
        {
            Console.WriteLine($"{client.displayName} has joined the chat room.");
        }

    }
}
