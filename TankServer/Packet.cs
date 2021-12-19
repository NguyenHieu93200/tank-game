using System;
using System.Collections.Generic;
using System.Text;

namespace TankServer
{
    public enum ServerPackets
    {
        sError,
        sConnect,
        sRoomInfo,
        sRoomList,
        sGameStart,
        sTankPosition,
        sTankShoot,
        sTankSpecial,
        sTankHealth,
        sTankDeath,
        sWinRound,
        sWinGame,
        sDisconnect
    }

    public enum ClientPackets
    {
        cConnect = 1,
        cCreateRoom,
        cRoomList,
        cJoinRoom,
        cInfoChange,
        cStartGame,
        cTankMove,
        cTankShoot,
        cTankSpecial,
        cTankHealth,
        cTankDeath,
        cWinRound,
        cWinGame,
        cDisconnect
    }

    internal class Packet
    {
        private int pos = 0;
        private readonly List<byte> buffer;
        private byte[] data;

        public Packet()
        {
            pos = 0;
            buffer = new List<byte>();
            data = buffer.ToArray();
        }

        public Packet(byte[] _data)
        {
            pos = 0;
            buffer = new List<byte>();
            buffer.AddRange(_data);
            data = buffer.ToArray();
        }

        public Packet(byte direction, byte messageCode)
        {
            pos = 0;
            buffer = new List<byte> { direction, messageCode };
            data = buffer.ToArray();
        }

        public byte[] ToArray()
        {
            data = buffer.ToArray();
            return data;
        }

        public string ToHexString()
        {
            data = buffer.ToArray();
            return BitConverter.ToString(data).Replace("-", " ");
        }

        public void OverwriteHeader(byte direction, byte messageCode)
        {
            buffer[0] = direction;
            buffer[1] = messageCode;
        }

        public void Write(byte _data)
        {
            buffer.Add(_data);
        }

        public void Write(bool _data)
        {
            buffer.AddRange(BitConverter.GetBytes(_data));
        }

        public void Write(int _data)
        {
            byte[] _converted = BitConverter.GetBytes(_data);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(_converted);
            }
            buffer.AddRange(_converted);
        }

        public void Write(float _data)
        {
            byte[] _converted = BitConverter.GetBytes(_data);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(_converted);
            }
            buffer.AddRange(_converted);
        }

        //public void Write(string _data)
        //{
        //    buffer.AddRange(Encoding.ASCII.GetBytes(_data.PadRight(100, '\0')));
        //}

        public void Write(string _data)
        {
            Write(_data.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(_data));
        }

        public void Write(byte[] _data)
        {
            buffer.AddRange(_data);
        }

        public void Write(Packet _data)
        {
            buffer.AddRange(_data.ToArray());
        }


        public byte ReadByte()
        {
            if (pos < data.Length)
            {
                return data[pos++];
            }
            else
            {
                throw new Exception("Can't read byte from buffer.");
            }
        }

        public bool ReadBoolean()
        {
            if (pos < data.Length)
            {
                bool val = BitConverter.ToBoolean(data, pos);
                pos++;
                return val;
            }
            else
            {
                throw new Exception("Can't read boolean from buffer.");
            }
        }

        public int ReadInt()
        {
            if (pos + 4 <= data.Length)
            {
                int val = BitConverter.ToInt32(data, pos);
                if (BitConverter.IsLittleEndian)
                {
                    byte[] _converted = BitConverter.GetBytes(val);
                    Array.Reverse(_converted);
                    val = BitConverter.ToInt32(_converted, 0);
                }
                pos += 4;
                return val;
            }
            else
            {
                throw new Exception("Can't read int from buffer.");
            }
        }

        public float ReadFloat()
        {
            if (pos + 4 <= data.Length)
            {
                float val = BitConverter.ToSingle(data, pos);
                if (BitConverter.IsLittleEndian)
                {
                    byte[] _converted = BitConverter.GetBytes(val);
                    Array.Reverse(_converted);
                    val = BitConverter.ToSingle(_converted, 0);
                }
                pos += 4;
                return val;
            }
            else
            {
                throw new Exception("Can't read float from buffer.");
            }
        }

        //public string ReadString()
        //{
        //    if (pos + 100 <= data.Length)
        //    {
        //        string val = Encoding.ASCII.GetString(data, pos, 100);
        //        pos += 100;
        //        return val;
        //    }
        //    else
        //    {
        //        throw new Exception("Can't read string from buffer.");
        //    }
        //}

        public string ReadString()
        {
            int length = ReadInt();
            if (pos + length <= data.Length)
            {
                string val = Encoding.ASCII.GetString(data, pos, length);
                pos += length;
                return val;
            }
            else
            {
                throw new Exception("Can't read string from buffer.");
            }
        }
    }
}
