using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TankClient
{
    internal class Client
    {
        public static Client instance = null;
        public const int DataBufferSize = 4096;
        public readonly int port;
        public readonly string ip;
        public TCP tcp;
        public int id;
        public string username;

        public Client(string _ip, int _port)
        {
            instance = this;
            ip = _ip;
            port = _port;
            tcp = new TCP(_ip, _port);
            tcp.Connect();
        }

        public class TCP
        {
            public TcpClient socket;
            private readonly string ip;
            private readonly int port;

            private NetworkStream stream;
            private byte[] buffer;

            public TCP(string _ip, int _port)
            {
                ip = _ip;
                port = _port;
            }

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = DataBufferSize,
                    SendBufferSize = DataBufferSize
                };

                buffer = new byte[DataBufferSize];
                socket.Connect(ip, port);
                if (!socket.Connected)
                {
                    Console.WriteLine("Can't connect to server. Please try again.");
                    return;
                }
                Console.WriteLine("Connected to server.");
                stream = socket.GetStream();

                stream.BeginRead(buffer, 0, DataBufferSize, ReceiveCallback, null);
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _bufferSize = stream.EndRead(_result);
                    if (_bufferSize <= 0)
                    {
                        instance.Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_bufferSize];
                    
                    Array.Copy(buffer, _data, _bufferSize);

                    PacketHandler.Handle(_data);

                    stream.BeginRead(buffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine(_ex);
                    instance.Disconnect();
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
                    }
                }
                catch (Exception _ex)
                {
                    instance.Disconnect();
                    Console.WriteLine($"Error sending data to server via TCP: {_ex}");
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
            tcp.Disconnect();
            instance = null;
            Console.WriteLine("Disconnected.");
        }
    }
}
