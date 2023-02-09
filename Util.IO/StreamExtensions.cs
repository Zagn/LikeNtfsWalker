﻿using System;
using System.IO;
using System.Text;

namespace Util.IO
{
    public static class StreamExtention
    {
        public static short ReadInt16(this Stream stream, int size = 2)
        {
            byte[] buffer = new byte[sizeof(short)];
            stream.Read(buffer, 0, Math.Min(size, buffer.Length));

            return BitConverter.ToInt16(buffer, 0);
        }

        public static int ReadInt32(this Stream stream, int size = 4)
        {
            byte[] buffer = new byte[sizeof(int)];
            stream.Read(buffer, 0, Math.Min(size, buffer.Length));

            return BitConverter.ToInt32(buffer, 0);
        }
        public static long ReadInt64(this Stream stream, int size = 8)
        {
            byte[] buffer = new byte[sizeof(long)];
            stream.Read(buffer, 0, Math.Min(size, buffer.Length));

            return BitConverter.ToInt64(buffer, 0);
        }

        public static string ReadString(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);

            return Encoding.UTF8.GetString(buffer);
        }
        public static byte[] ReadBytes(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);

            return buffer;
        }

        public static bool ReadBool(this Stream stream)
        {
            return stream.ReadByte() == 1;
        }
    }
}
