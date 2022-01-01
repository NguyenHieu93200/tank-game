using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TankClient;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public static Client instance = null;
    public const int DataBufferSize = 4096;
    public readonly int port = 8000;
    public string ip = "127.0.0.1";
    public TCP tcp;
    public int id;
    public string username;

    public int roomId;
    public string roomName;
    public int hostId;

    public int count1 = 0 ;
    public int count2 = 0;
    public byte team;

    public List<PlayerInfo> players;
    public List<Room> rooms;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    private void OnApplicationQuit()
    {
        Disconnect(); // Disconnect when the game is closed
    }
    private void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void Connect(string _username, string _ip)
    {
        ip = _ip;
        username = _username;
        tcp = new TCP(ip, port);
        tcp.Connect();
        PacketSender.ConnectSender(username);
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
                Debug.Log("Can't connect to server. Please try again.");
                return;
            }
            Debug.Log("Connected to server.");
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

                stream.BeginRead(buffer, 0, DataBufferSize, ReceiveCallback, null);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    PacketHandler.Handle(_data);
                });
                
            }
            catch (Exception _ex)
            {
                Debug.Log(_ex.Message);
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
                Debug.Log($"Error sending data to server via TCP: {_ex.Message}");
            }
        }

        public void Disconnect()
        {
            if (socket != null)
            {
                socket.Close();
            }
            stream = null;
            buffer = null;
            socket = null;
        }
    }

    public void Disconnect()
    {
        if (tcp != null)
        {
            tcp.Disconnect();
        }
        instance = null;
        Debug.Log("Disconnected.");
    }
}

