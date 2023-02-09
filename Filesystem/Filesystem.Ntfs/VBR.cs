﻿using System;
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
        public int JmpCommand;
        public string OemID;
        public short BytesPerSector;
        public short SectorsPerCluster;
        public short Reserved;
        public short Always0_0;
        //public int Unused0; 의미x 분석x
        public short MediaDescriptor;
        public short Always0_1;
        public short SectorPerTrack;
        public short NumberofHeads;
        public int HiddenSectors;
        //public int Unused1; 
        //public int Unused2; 
        public long TotalSectors;
        public long LogicalClusterNumberForTheFileMFT;
        public long LogicalClusterNumberForTheFileMFTMirr;
        public int ClustersPerFileRecordSegment;
        public int ClustersPerIndexBlock;
        public long VolumeSerialNumber;
        public int Checksum;
        //public int BootCodeAndErrorMessage; //분석x
        public int Signature;
        public byte[] Data;

        public int ClusterSize => BytesPerSector * SectorsPerCluster;
        public long MftStartOffset => ClusterSize * LogicalClusterNumberForTheFileMFT;

        public VBR(Stream stream)
        {
            //byte[] buffer = new byte[512]
            //stream.Read(buffer, 0, 3);
            //JmpCommand = ByteConverter_2.ToInt16(buffer); //<- 4바이트를 읽어서 16,32,64 / 3바이트만 읽을 수 있게 바꿔보기 /stream.ReadInt32(3)

            JmpCommand = stream.ReadInt32();
            OemID = stream.ReadString(8);
            BytesPerSector = stream.ReadInt16();
            SectorsPerCluster = stream.ReadInt16();
            Reserved = stream.ReadInt16();
            Always0_0 = stream.ReadInt16();
            stream.Seek(2, SeekOrigin.Current); //Unused0
            MediaDescriptor = stream.ReadInt16();
            Always0_1 = stream.ReadInt16();
            SectorPerTrack = stream.ReadInt16();
            NumberofHeads = stream.ReadInt16();
            HiddenSectors = stream.ReadInt32();
            stream.Seek(8, SeekOrigin.Current); //Unused1, Unused2
            TotalSectors = stream.ReadInt64();
            LogicalClusterNumberForTheFileMFT = stream.ReadInt64();
            LogicalClusterNumberForTheFileMFTMirr = stream.ReadInt64();
            ClustersPerFileRecordSegment = stream.ReadInt32();
            ClustersPerIndexBlock = stream.ReadInt32();
            VolumeSerialNumber = stream.ReadInt64();
            Checksum = stream.ReadInt32();
            stream.Seek(426, SeekOrigin.Current);
            Signature = stream.ReadInt32();
        }


    }
}
