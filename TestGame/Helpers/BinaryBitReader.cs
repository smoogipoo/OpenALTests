using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Helpers
{
    public class BinaryBitReader : IDisposable
    {
        public Stream BaseStream;

        private byte[] buffer;
        private uint currentBit;

        public BinaryBitReader(Stream stream)
        {
            BaseStream = stream;
        }

        public int PeekByte()
        {
            int res = (int)BaseStream.ReadByte();
            if (res != -1)
                BaseStream.Position--;
            return res;
        }

        public T[] Read<T>(int length) where T : struct
        {
            fillBuffer(length * Marshal.SizeOf(typeof(T)) * 8);

            T[] ret = new T[buffer.Length];
            Array.Copy(buffer, ret, buffer.Length);

            return ret;
        }

        public byte[] ReadBytes(int length)
        {
            return Read<byte>(length);
        }

        public char[] ReadChars(int length)
        {
            return Read<char>(length);
        }

        public byte ReadByte()
        {
            fillBuffer(8);
            return buffer[0];
        }

        public char ReadChar()
        {
            return (char)ReadByte();
        }

        public int ReadInt16()
        {
            fillBuffer(16);
            return (short)(buffer[1] << 8 | buffer[0]);
        }

        public ushort ReadUInt16()
        {
            return (ushort)ReadInt16();
        }

        public int ReadInt24()
        {
            fillBuffer(24);
            return (int)(buffer[2] << 16 | buffer[1] << 8 | buffer[0]);
        }

        public uint ReadUInt24()
        {
            return (uint)ReadInt24();
        }

        public int ReadInt32()
        {
            fillBuffer(32);
            return (int)(buffer[3] << 24 | buffer[2] << 16 | buffer[1] << 8 | buffer[0]);
        }

        public uint ReadUInt32()
        {
            return (uint)ReadInt32();
        }

        public int ReadIntN(int numBits)
        {
            fillBuffer(numBits);

            int byteCount = (int)Math.Ceiling(numBits / 8f);

            int ret = 0;
            for (int i = 0; i < byteCount; i++)
                ret |= buffer[i] << 8 * i;

            return ret;
        }

        public uint ReadUIntN(int numBits)
        {
            return (uint)ReadIntN(numBits);
        }

        private void fillBuffer(int numBits)
        {
            //Make space in our buffer for the requested bit length
            int byteCount = (int)Math.Ceiling(numBits / 8f);
            if (buffer == null)
                buffer = new byte[byteCount];
            else
            {
                Array.Resize(ref buffer, byteCount);
                Array.Clear(buffer, 0, buffer.Length);
            }

            //Read from stream
            int currentByte = (int)Math.Floor(currentBit / 8f);
            int bytesToRead = (int)Math.Min(byteCount, BaseStream.Length - currentByte);
            BaseStream.Read(buffer, 0, bytesToRead);

            //BS the last byte for non-MO8 numBits
            if (numBits % 8 > 0)
                buffer[buffer.Length - 1] >>= (8 - numBits % 8);

            currentBit += (uint)bytesToRead * 8;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                BaseStream.Dispose();
        }
    }
}
