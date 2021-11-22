using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace server
{
    internal class server
    {
        public int id;
        public TcpClient tcp;
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static TcpListener serverListener;
        public static SortedDictionary<int, Client> clients = new SortedDictionary<int, Client>();

        public static void SetupServer(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;
            ServerData();

            serverListener = new TcpListener(IPAddress.Any, Port);
            Console.WriteLine($"Server was created!\nPort is {Port}.");
        }
        public static void StartServer()
        {
            serverListener.Start();
            Console.WriteLine($"Server is begun.");

            serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);
            Console.WriteLine("Client is waiting....");
        }
        public static void AcceptClientCallBack(IAsyncResult asyncResult)
        {
            TcpClient socket = serverListener.EndAcceptTcpClient(asyncResult);
            serverListener.BeginAcceptTcpClient(AcceptClientCallBack, null);

            Console.WriteLine($"Client is connecting... {socket.Client.RemoteEndPoint}");
            for(int i = 1; i <= clients.Count; i++)
            {

                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(socket);
                    return;
                }
            }
            socket.Close();
            Console.WriteLine("Client could not connected!");
        }
        public static void ServerData()
        {
            clients.Add(1,new Client(1));
            
        }
        static void Main(string[] args)
        {
            SetupServer(1,8080);
            StartServer();
            Console.ReadKey();
        }
    }
}
