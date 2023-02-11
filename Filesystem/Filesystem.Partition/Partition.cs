using System;
using System.IO;

namespace Filesystem.Partition
{
    public class Partition
    {
        public byte BootFlag;
        public uint StartingCHSAddr;
        public byte PartitionType;
        public uint EndingCHSAddr;
        public uint StartingLBAAddr;
        public uint SizeInSector;

        byte[] buff = new byte[4];
        public Partition(Stream stream)
        {
            stream.Read(buff, 0, 1);
            BootFlag = buff[0];
            Array.Clear(buff, 0, 1);

            stream.Read(buff, 0, 3);
            StartingCHSAddr = BitConverter.ToUInt32(buff, 0);
            Array.Clear(buff, 0, 3);

            stream.Read(buff, 0, 1);
            PartitionType = buff[0];
            Array.Clear(buff, 0, 1);

            stream.Read(buff, 0, 3);
            EndingCHSAddr = BitConverter.ToUInt32(buff, 0);
            Array.Clear(buff, 0, 3);

            stream.Read(buff, 0, 4);
            StartingLBAAddr = BitConverter.ToUInt32(buff, 0);
            Array.Clear(buff, 0, 4);

            stream.Read(buff, 0, 4);
            SizeInSector = BitConverter.ToUInt32(buff, 0);
            Array.Clear(buff, 0, 4);

        }
    }
}
