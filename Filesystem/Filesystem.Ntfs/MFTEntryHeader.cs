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
        public short OffsetOfFixupArray;
        public short CountOfFixupValues;
        public long LogFileSequenceNumber;
        public short SequenceNumber;
        public short LinkCount;
        public short OffsetToFirstAttribute;
        public short Flags;
        public int UsedSizeofMFTEntry;
        public int AllocatedSizeOfMFTEntry;
        public long FileReferenceToBaseMFTEntry;
        public short NextAttributeID;
        public short AlignTo4BBoundary;
        public int NumberOfThisMFTEntry;

        public MFTEntryHeader(Stream stream)
        {
            //byte[] buffer = new byte[512];
            //stream.Read(buffer, 0, 4);
            //Signature = ByteConverter_string.ToString(buffer);       

            Signature = stream.ReadString(8);
            OffsetOfFixupArray = stream.ReadInt16();
            CountOfFixupValues = stream.ReadInt16();
            LogFileSequenceNumber = stream.ReadInt64();
            SequenceNumber = stream.ReadInt16();
            LinkCount = stream.ReadInt16();
            OffsetToFirstAttribute = stream.ReadInt16();
            Flags = stream.ReadInt16();
            UsedSizeofMFTEntry = stream.ReadInt32();
            AllocatedSizeOfMFTEntry = stream.ReadInt32();
            FileReferenceToBaseMFTEntry = stream.ReadInt64();
            NextAttributeID = stream.ReadInt16();
            AlignTo4BBoundary = stream.ReadInt16();
            NumberOfThisMFTEntry = stream.ReadInt32();
        }
    }
}
