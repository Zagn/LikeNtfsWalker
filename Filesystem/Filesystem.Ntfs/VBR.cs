using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.IO;

namespace Filesystem.Ntfs
{
    public class VBR
    {
        public uint JmpCommand;
        public string OemID;
        public ushort BytesPerSector;
        public ushort SectorsPerCluster;
        public ushort Reserved;
        public ushort Always0_0;
        //public int Unused0; 의미x 분석x
        public ushort MediaDescriptor;
        public ushort Always0_1;
        public ushort SectorPerTrack;
        public ushort NumberofHeads;
        public uint HiddenSectors;
        //public int Unused1; 
        //public int Unused2; 
        public ulong TotalSectors;
        public ulong LogicalClusterNumberForTheFileMFT;
        public ulong LogicalClusterNumberForTheFileMFTMirr;
        public uint ClustersPerFileRecordSegment;
        public uint ClustersPerIndexBlock;
        public ulong VolumeSerialNumber;
        public uint Checksum;
        //public int BootCodeAndErrorMessage; //분석x
        public ushort Signature;
        public byte[] Data;

        public uint ClusterSize => (uint)BytesPerSector * SectorsPerCluster;
        public ulong MftStartOffset;

  
        public VBR(Stream stream)
        {
            //byte[] buffer = new byte[512]
            //stream.Read(buffer, 0, 3);
            //JmpCommand = ByteConverter_2.ToInt16(buffer); //<- 4바이트를 읽어서 16,32,64 / 3바이트만 읽을 수 있게 바꿔보기 /stream.ReadInt32(3)
            MftStartOffset = ClusterSize * LogicalClusterNumberForTheFileMFT + (ulong)stream.Position;

            JmpCommand = stream.ReadUInt32(3);
            OemID = stream.ReadString(8);
            BytesPerSector = stream.ReadUInt16();
            SectorsPerCluster = stream.ReadUInt16(1);
            Reserved = stream.ReadUInt16();
            Always0_0 = stream.ReadUInt16();
            stream.Seek(2, SeekOrigin.Current); //Unused0
            MediaDescriptor = stream.ReadUInt16();
            Always0_1 = stream.ReadUInt16();
            SectorPerTrack = stream.ReadUInt16();
            NumberofHeads = stream.ReadUInt16();
            HiddenSectors = stream.ReadUInt32();
            stream.Seek(8, SeekOrigin.Current); //Unused1, Unused2
            TotalSectors = stream.ReadUInt64();
            LogicalClusterNumberForTheFileMFT = stream.ReadUInt64();
            LogicalClusterNumberForTheFileMFTMirr = stream.ReadUInt64();
            ClustersPerFileRecordSegment = stream.ReadUInt32();
            ClustersPerIndexBlock = stream.ReadUInt32();
            VolumeSerialNumber = stream.ReadUInt64();
            Checksum = stream.ReadUInt32();
            stream.Seek(426, SeekOrigin.Current);
            Signature = stream.ReadUInt16();
        }
    }
}
