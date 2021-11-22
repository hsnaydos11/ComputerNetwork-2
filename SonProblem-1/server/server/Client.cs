using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace server
{
    class Client
    {
        private static int bufferSize = 4096;
        public int id;
        public TCP tcp;


        public Client(int _id)
        {
            id = _id;
            tcp= new TCP(id);
        }
        public void Disconnect()
        {
            tcp.Disconnect();
        }
        public class TCP
        {
            public TcpClient socket;
            public readonly int id;
            public NetworkStream stream;
            public byte[] buffer;
            public TCP(int _id)
            {
                id = _id;
            }
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = bufferSize;
                socket.SendBufferSize = bufferSize;

                stream = socket.GetStream();

                buffer = new byte[bufferSize];
                stream.BeginRead(buffer,0,buffer.Length,new AsyncCallback(ReceiveCallBack), null);
                Console.WriteLine($"Connected {id}");
            }
            public void Disconnect()
            {
                stream = null;
                socket = null;
                buffer = null;
                Console.WriteLine($"{id}.Client exitted");
            }
            public void ReceiveCallBack(IAsyncResult asyncResult)
            {
                try
                {
                    int receivedData = stream.EndRead(asyncResult);
                    if(receivedData <= 0)
                    {
                        server.clients[id].Disconnect();
                        return;
                    }
                    byte[]_data=new byte[receivedData];
                    Array.Copy(buffer, _data, receivedData);

                    string datareceive = Encoding.UTF8.GetString(_data);
                    Console.WriteLine($"{id}. Received message from client:{ datareceive}");
                }
                catch (Exception e)
                {
                    server.clients[id].Disconnect();
                }
            }
        }
    }
}
