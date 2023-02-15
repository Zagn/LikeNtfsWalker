using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Util.IO;
using Filesystem.Partition;
using System.Net.Sockets;

namespace Filesystem.Partition
{
    public class VolumeLable
    {
        public static string FromNtfs(Stream stream) //partition stream //VBR start location
        {
            const int volumeEntryOffset = 512 * 3 * 2;
            ushort bytePerSector;
            ushort sectorPerCluster;
            long logicalClusterNumberForTheFileMFT;
            ushort firstAttrOffset;

            stream.Seek(11, SeekOrigin.Begin);
            bytePerSector = stream.ReadUInt16();
            sectorPerCluster = stream.ReadUInt16(1);

            stream.Seek(48, SeekOrigin.Begin);
            logicalClusterNumberForTheFileMFT = stream.ReadInt64();

            uint ClusterSize = (uint)(sectorPerCluster * bytePerSector);
            long mftStartOffset = logicalClusterNumberForTheFileMFT * ClusterSize;


            stream.Seek(mftStartOffset + volumeEntryOffset, SeekOrigin.Begin); //location of $Volume
            stream.Seek(20, SeekOrigin.Current); 
            
            firstAttrOffset = stream.ReadUInt16(); //Read First Offset 
            
            stream.Seek(mftStartOffset + volumeEntryOffset + firstAttrOffset, SeekOrigin.Begin);
            
            //MftStartOffset = ((ulong)ClusterSize * LogicalClusterNumberForTheFileMFT);
            //MftEntry mftEntry = new MftEntry(stream, vbr.ClusterSize);

            var attrStartPos = stream.Position;

            for (int i = 0; i < 10; i++)
            {
                stream.Position = attrStartPos;
                var attrType = stream.ReadUInt32();
                var attrLen = stream.ReadUInt32();

                if (attrType == 96)
                {
                    stream.Position = attrStartPos + 16;
                    var sizeOfCont = stream.ReadInt32();
                    var offsetOfCont = stream.ReadUInt16();
                    stream.Position = attrStartPos + offsetOfCont;

                    return stream.ReadString(sizeOfCont).Replace("\0", "");
                }
                else
                {
                    attrStartPos += attrLen;
                }
            }
            return "<null>";
        }
    }
}

