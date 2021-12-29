using System;
using System.Net.Sockets;

namespace TankServer
{
    internal class Client
    {
        public const int DataBufferSize = 4096;

        public int id;
        public string username;
        public int roomId;
        public byte teamId;
        public byte tankId;
        public TCP tcp;

        public Client(int _id)
        {
            id = _id;
            roomId = 0;
            tcp = new TCP(id);
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] buffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;

                socket.ReceiveBufferSize = DataBufferSize;
                socket.SendBufferSize = DataBufferSize;

                stream = socket.GetStream();
                buffer = new byte[DataBufferSize];

                stream.BeginRead(buffer, 0, DataBufferSize, DataReceiveCallback, null);
            }

            private void DataReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _bufferSize = stream.EndRead(_result);
                    if (_bufferSize <= 0)
                    {
                        Server.clients[id].Disconnect();
                        return;
                    }
                    byte[] _data = new byte[_bufferSize];

                    Array.Copy(buffer, _data, _bufferSize);

                    stream.BeginRead(buffer, 0, DataBufferSize, DataReceiveCallback, null);

                    ServerHandler.Handle(id, _data);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine(_ex.Message);
                    Server.clients[id].Disconnect();
                }
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    byte[] _buffer = _packet.ToArray();
                    if (socket != null)
                    {
                        stream.BeginWrite(_buffer, 0, _buffer.Length, null, null);
                        Console.Write($"Send to {id}: ");
                        Console.WriteLine(_packet.ToHexString());
                    }
                }
                catch (Exception _ex)
                {
                    Server.clients[id].Disconnect();
                    Console.WriteLine($"Error sending data to server via TCP: {_ex.Message}");
                }
            }

            public void Disconnect()
            {
                socket.Close();
                stream = null;
                buffer = null;
                socket = null;
            }
        }

        public void Disconnect()
        {
            if (roomId != 0)
            {
                Server.rooms[roomId].RemoveClient(Server.clients[id]);
            }
            Console.WriteLine($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");
            username = null;
            tcp.Disconnect();
        }
    }
}
