using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    internal class Client
    {
        public static string HOST { get; private set; }
        public static int PORT { get; private set; }

        public static NetworkStream stream;
        public static TcpClient socket = new TcpClient();
        public static void ServerSettings(string _host, int _port)
        {
            HOST = _host;
            PORT = _port;   
        }
        public static void Connect()
        {
            socket.BeginConnect(Client.HOST, Client.PORT,new AsyncCallback(ConnectCallBack),null);
        }
        public static void ConnectCallBack(IAsyncResult asyncResult)
        {
            socket.EndConnect(asyncResult);
            if (socket.Connected)
            {
                stream = socket.GetStream();
                Console.WriteLine("Client connected!");
                Console.Write("Enter the text: ");
                string text = Console.ReadLine();
                SendMessage(text);
            }
        }

        public static void SendMessage(string message)
        {
            byte[] _data = Encoding.UTF8.GetBytes(message);

            try
            {
                stream.BeginWrite(_data, 0, _data.Length,SendCallBack ,null);
            }
            catch
            {
                Console.WriteLine("There is ERROR");
            }
        }
        public static void SendCallBack(IAsyncResult asyncResult)
        {
            stream.EndWrite(asyncResult);
        }
        static void Main(string[] args)
        {
            ServerSettings("95.183.174.159", 8080);

            Connect();
            Console.ReadKey();
        }
    }
}
