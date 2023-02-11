using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.IO;

namespace Filesystem.Ntfs
{
    public class MFTEntryHeader
    {
        public string Signature;
        public ushort OffsetOfFixupArray;
        public ushort CountOfFixupValues;
        public ulong LogFileSequenceNumber;
        public ushort SequenceNumber;
        public ushort LinkCount;
        public ushort OffsetToFirstAttribute;
        public ushort Flags;
        public uint UsedSizeofMFTEntry;
        public uint AllocatedSizeOfMFTEntry;
        public ulong FileReferenceToBaseMFTEntry;
        public ushort NextAttributeID;
        public ushort AlignTo4BBoundary;
        public uint NumberOfThisMFTEntry;

        public MFTEntryHeader(Stream stream)
        {
            //byte[] buffer = new byte[512];
            //stream.Read(buffer, 0, 4);
            //Signature = ByteConverter_string.ToString(buffer);       

            Signature = stream.ReadString(4);
            OffsetOfFixupArray = stream.ReadUInt16();
            CountOfFixupValues = stream.ReadUInt16();
            LogFileSequenceNumber = stream.ReadUInt64();
            SequenceNumber = stream.ReadUInt16();
            LinkCount = stream.ReadUInt16();
            OffsetToFirstAttribute = stream.ReadUInt16();
            Flags = stream.ReadUInt16();
            UsedSizeofMFTEntry = stream.ReadUInt32();
            AllocatedSizeOfMFTEntry = stream.ReadUInt32();
            FileReferenceToBaseMFTEntry = stream.ReadUInt64();
            NextAttributeID = stream.ReadUInt16();
            AlignTo4BBoundary = stream.ReadUInt16();
            NumberOfThisMFTEntry = stream.ReadUInt32();
        }
    }
}
