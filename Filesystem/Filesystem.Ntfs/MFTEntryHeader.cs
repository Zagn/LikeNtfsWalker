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
        public long LogFileSequenceNumber;
        public ushort SequenceNumber;
        public ushort LinkCount;
        public ushort OffsetToFirstAttribute;
        public ushort Flags;
        public int UsedSizeofMFTEntry;
        public int AllocatedSizeOfMFTEntry;
        public long FileReferenceToBaseMFTEntry;
        public ushort NextAttributeID;
        public ushort AlignTo4BBoundary;
        public int NumberOfThisMFTEntry;

        public MFTEntryHeader(Stream stream)
        {
            //byte[] buffer = new byte[512];
            //stream.Read(buffer, 0, 4);
            //Signature = ByteConverter_string.ToString(buffer);       

            Signature = stream.ReadString(8);
            OffsetOfFixupArray = stream.ReadUInt16();
            CountOfFixupValues = stream.ReadUInt16();
            LogFileSequenceNumber = stream.ReadInt64();
            SequenceNumber = stream.ReadUInt16();
            LinkCount = stream.ReadUInt16();
            OffsetToFirstAttribute = stream.ReadUInt16();
            Flags = stream.ReadUInt16();
            UsedSizeofMFTEntry = stream.ReadInt32();
            AllocatedSizeOfMFTEntry = stream.ReadInt32();
            FileReferenceToBaseMFTEntry = stream.ReadInt64();
            NextAttributeID = stream.ReadUInt16();
            AlignTo4BBoundary = stream.ReadUInt16();
            NumberOfThisMFTEntry = stream.ReadInt32();
        }
    }
}
