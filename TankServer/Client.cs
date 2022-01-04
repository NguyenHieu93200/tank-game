using System;
using System.Net;
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
        public UDP udp;

        public Client(int _id)
        {
            id = _id;
            roomId = 0;
            tcp = new TCP(id);
            udp = new UDP(id);
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

        public class UDP
        {
            public IPEndPoint endPoint;

            private readonly int id;

            public UDP(int _id)
            {
                id = _id;
            }

            public void Connect(IPEndPoint _endPoint)
            {
                endPoint = _endPoint;
            }

            /// <summary>Sends data to the client via UDP.</summary>
            /// <param name="_packet">The packet to send.</param>
            public void SendData(Packet _packet)
            {
                Server.SendUDPData(endPoint, _packet);
            }

            /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
            /// <param name="_packetData">The packet containing the recieved data.</param>
            public void HandleData(Packet _packetData)
            {
                ServerHandler.Handle(id, _packetData.ToArray());
            }

            public void Disconnect()
            {
                endPoint = null;
            }
        }

        public void Disconnect()
        {
            if (roomId != 0)
            {   
                ServerSender.DisconnectSender(id, roomId);
                Server.rooms[roomId].RemoveClient(Server.clients[id]);
            }
            if (tcp.socket != null)
            {
                Console.WriteLine($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");
                tcp.Disconnect();
            }
            
            username = null;
            
        }
    }
}
