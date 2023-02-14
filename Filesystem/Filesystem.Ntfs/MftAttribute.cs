using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Util.IO;

namespace Filesystem.Ntfs
{
    public enum AttType
    {
        Unknown,
        SIA = 16,
        AttributeList = 0x20,
        FileName = 48,
        ObjectID = 0x40,
        SecurityDescriptor = 0x50,
        VolumeName = 96,
        VolumeInformation = 0x70,
        Data = 128,
        IndexRoot = 0x90,
        IndexAllocation = 0xA0,
        Bitmap = 176,
        SymbolicLink = 0xC0,
        EA_Information = 0xD0,
        EA = 0xE0,
        LoggedUtilityStream = 0xF0
    }

    internal class MftAttribute
    {
        public MftAttHeader Header { get; }
        public AttType Type => Header.Type;

        public MftAttribute(MftAttHeader header)
        {
            Header = header;
        }
    }

    internal class MftAttHeader
    {
        public AttType Type { get; }

        //public int AttType;
        public uint AttLength { get; }
        public ushort NonResidentFlag { get; }
        public ushort NameLength { get; }
        public ushort OffsetToTheName { get; }
        public ushort Flag { get; }
        public ushort AttID { get; }

        public MftAttHeader(Stream stream)
        {
            //var t = AttType.Unknown;
            //var value = 0x10;
            //if (Enum.IsDefined(typeof(AttType), value))
            //    t = (AttType)value;

            Type = AttType.Unknown;
            var value = stream.ReadInt32();
            if (Enum.IsDefined(typeof(AttType), value))
                Type = (AttType)value;

            AttLength = stream.ReadUInt32();
            NonResidentFlag = stream.ReadUInt16(1);
            NameLength = stream.ReadUInt16(1);
            OffsetToTheName = stream.ReadUInt16();
            Flag = stream.ReadUInt16();
            AttID = stream.ReadUInt16();
        }
    }

    internal class ResidentHeader : MftAttHeader
    {
        public uint SizeOfContent { get; }
        public ushort OffsetOfCount { get; }
        public bool IsIndex { get; }
        public byte Padding { get; }

        public ResidentHeader(Stream stream) : base(stream)
        {
            SizeOfContent = stream.ReadUInt32();
            OffsetOfCount = stream.ReadUInt16();
            //IsIndex = buffer[0] == 1;
            IsIndex = stream.ReadBool();
            Padding = (byte)stream.ReadByte();
        }
    }

    internal class NonResidentHeader : MftAttHeader
    {
        public ulong StartVcn { get; }
        public ulong EndVcn { get; }
        public ushort RunlistOffset { get; }
        public ushort CompressUnitSize { get; }
        public uint Padding { get; }
        public ulong ContentAllocSize { get; }
        public ulong ContentActureSize { get; }
        public ulong ContentInitSize { get; }

        public List<ClusterRun> ClusterRuns = new List<ClusterRun>();

        public NonResidentHeader(Stream stream) : base(stream)
        {

            StartVcn = stream.ReadUInt64();

            EndVcn = stream.ReadUInt64();

            RunlistOffset = stream.ReadUInt16();

            CompressUnitSize = stream.ReadUInt16();

            Padding = stream.ReadUInt32();

            ContentAllocSize = stream.ReadUInt64();

            ContentActureSize = stream.ReadUInt64();

            ContentInitSize = stream.ReadUInt64();

            var RunlistLength = AttLength - RunlistOffset;

            int i = 0;
            while (i < RunlistLength)
            {
                byte ClusterrunHeader = (byte)stream.ReadByte();
                if (ClusterrunHeader == 0x00)
                {
                    stream.Seek(RunlistLength - (i + 1), SeekOrigin.Current);
                    break;
                }

                int RunOffsetSize = ClusterrunHeader >> 4;
                int RunLengthSize = ClusterrunHeader & 0x0F;

                var RunLength = stream.ReadUInt64(RunLengthSize);
                var RunOffset = stream.ReadInt64(RunOffsetSize);

                i +=  1 + RunLengthSize + RunOffsetSize;

                ClusterRuns.Add(new ClusterRun(RunLength, RunOffset));

            }

            

        }
    }

    internal class ClusterRun
    {
        public ulong RunLength { get; }
        public long RunOffset { get; }

        public ClusterRun(ulong runLength, long runOffset)
        {
            RunLength = runLength;
            RunOffset = runOffset;
        }

    }
    internal class StandardInformation : MftAttribute
    {
        public ulong CreationTime { get; }
        public ulong ModifiedTime { get; }
        public ulong MFTModifiedTime { get; }
        public ulong LastAccessedTime { get; }
        public uint Flags { get; }
        public uint MaximumNumberOfVersions { get; }
        public uint VersionNumber { get; }
        public uint ClassID { get; }
        public uint OwnerID { get; }
        public uint SecurityID { get; }
        public ulong QuotaCharged { get; }
        public ulong UpdateSequenceNumber { get; }

        public StandardInformation(MftAttHeader header, Stream stream) : base(header)
        {          
            CreationTime = stream.ReadUInt64();
            ModifiedTime = stream.ReadUInt64();
            MFTModifiedTime = stream.ReadUInt64();
            LastAccessedTime = stream.ReadUInt64();
            Flags = stream.ReadUInt32();
            MaximumNumberOfVersions = stream.ReadUInt32();
            VersionNumber = stream.ReadUInt32();
            ClassID = stream.ReadUInt32();
            OwnerID = stream.ReadUInt32();
            SecurityID = stream.ReadUInt32();
            QuotaCharged = stream.ReadUInt64();
            UpdateSequenceNumber = stream.ReadUInt64();

        }
    }

    internal class FileName : MftAttribute
    {
        public ulong FileReferenceOfParentDirectory { get; }
        public ulong CreationTime { get; }
        public ulong ModifiedTime { get; }
        public ulong MFTModifiedTime { get; }
        public ulong LastAccessedTime { get; }
        public ulong AllocatedSizeOfFile { get; }
        public ulong RealSizeOfFile { get; }
        public uint Flags { get; }
        public uint ReparseValue { get; }
        public ushort LengthOfName { get; }
        public ushort Namespace { get; }
        public string Name { get; }
        public FileName(MftAttHeader header, Stream stream) : base(header)
        {
            stream.Seek(-24, SeekOrigin.Current);
            var filenamestartoffset = stream.Position;
            var s = filenamestartoffset + header.AttLength; //
            stream.Seek(24, SeekOrigin.Current);

            FileReferenceOfParentDirectory = stream.ReadUInt64();

            CreationTime = stream.ReadUInt64();

            ModifiedTime = stream.ReadUInt64();

            MFTModifiedTime = stream.ReadUInt64();

            LastAccessedTime = stream.ReadUInt64();

            AllocatedSizeOfFile = stream.ReadUInt64();

            RealSizeOfFile = stream.ReadUInt64();

            Flags = stream.ReadUInt32();

            ReparseValue = stream.ReadUInt32();

            LengthOfName = stream.ReadUInt16(1);
            var RealLengthOfName = LengthOfName * 2;

            Namespace = stream.ReadUInt16(1);
                   
            Name = stream.ReadString(RealLengthOfName);

            var pos = stream.Position;
            var FileNamePadding = s - pos;
            stream.Seek(FileNamePadding, SeekOrigin.Current);
        }
    }


    internal class Bitmap : MftAttribute
    {
        public byte[] BitFiled { get; }

        public Bitmap(MftAttHeader header, Stream stream) : base(header)
        {
            if (header.NonResidentFlag == 1)
                return;

            var regidentHeader = (ResidentHeader)header;

            BitFiled = new byte[regidentHeader.SizeOfContent];
            stream.Read(BitFiled, 0, BitFiled.Length);
        }
    }

    internal class VolumeName : MftAttribute
    {
        public byte[] UnicodeName { get; }

        public VolumeName(MftAttHeader header, Stream stream) : base(header)
        {
            if (header.NonResidentFlag == 1)
                return;

            var regidentHeader = (ResidentHeader)header;

            UnicodeName = new byte[regidentHeader.SizeOfContent];
            stream.Read(UnicodeName, 0, UnicodeName.Length);
        }
    }

    internal class DataAttribute : MftAttribute
    {
        //public List<ClusterRun> ClusterRuns { get; }

        public byte[] Data { get; }

        public DataAttribute(MftAttHeader header, Stream stream) : base(header)
        {
            if (header.NonResidentFlag == 1)
                return;

            // Resident 이면 Data에 값 입력
            var regidentHeader = (ResidentHeader)header;
            //Data = stream.ReadBytes(regidentHeader.SizeOfContent);

            Data = new byte[regidentHeader.SizeOfContent];
            stream.Read(Data, 0, Data.Length);
        }
    }
}

