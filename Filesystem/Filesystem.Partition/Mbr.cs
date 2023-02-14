using System;
using System.Collections.Generic;
using Util.IO;

namespace Filesystem.Partition
{
    public class Mbr
    {
        public const short SIG_OFFSET = 510;
        public short PART_START_OFFSET = 446;
        public bool isMbr;
        
        bool isLittle;
        long isPartitionNull;

        byte[] buff = new byte[8];
        byte[] buffToSig = new byte[2];

        public ushort signature;
        public List<Partition> partitions;

        DeviceStream stream;

        public Mbr(string path)
        {
            stream = new DeviceStream(path, 512);

            isLittle = BitConverter.IsLittleEndian;

            //Checking File 
            stream.Position = SIG_OFFSET;
            stream.Read(buffToSig, 0, 2);


            partitions = new List<Partition>();

            if (isLittle)
            {
                Array.Reverse(buffToSig);
            }

            signature = BitConverter.ToUInt16(buffToSig, 0); //isMbr
            if (signature == 21930)
            {
                isMbr = true;
            } else
            {
                isMbr = false;
            }

            for (int i = 0; i < 4; i++)
            {
                stream.Position = PART_START_OFFSET + (16 * i);
                stream.Read(buff, 0, 8);
                isPartitionNull = BitConverter.ToInt64(buff, 0);

                if (isPartitionNull == 0)
                {
                    continue;
                }
                else
                {
                    stream.Position = PART_START_OFFSET + (16 * i);
                    Partition partition = new Partition(stream);
                    partitions.Add(partition);
                }
            }

            stream.Close();
        }
    }
}
