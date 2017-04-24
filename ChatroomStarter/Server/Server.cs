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
        public static Dictionary<string, Client> activeChatClients = new Dictionary<string, Client>();
        TcpListener server;

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.107"), 6915);
            server.Start();
            Console.WriteLine("Chat Server Started");
        }
        public void Run()
        {
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
                client.GetDisplayName();
                //Thread chatThread = new Thread(() => StartChat(client));
                activeChatClients.Add(client.UserId, client);
                Thread chatThread = new Thread(new ThreadStart(client.Recieve));
                chatThread.Start();
                Console.WriteLine($"==> {client.DisplayName} joined the chat");
                Thread acceptMoreClients = new Thread(new ThreadStart(AcceptClient));
                acceptMoreClients.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while trying to create a threaded client");
            }

        }

        private void Respond()
        {
            while (true)
            {
                //foreach (KeyValuePair<string, Client> kvp in activeChatClients.ToList())
                foreach (var kvp in activeChatClients)
                    {
                    Message message = default(Message);
                    if (messageQueue.TryDequeue(out message))
                    {
                        client.Send(message.Body);
                        break;
                    }
                }

            }
        }

        private void NotifyNewUser(string displayName)
        {
            Console.WriteLine($"{client.DisplayName} has joined the chat room.");
        }

    }
}
