using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Util.IO;

namespace Filesystem.Ntfs
{
    
    public class FindVolumeLable
    {
        uint AttrType;
        uint AttrLen;
        long AttrStartPos;
        long MFTStartPos;
        int SizeOfCont;
        ushort OffsetOfCont;
        public string volumeLable;
        
        public FindVolumeLable(Stream stream) //get MFTStartOffset from VBR
        {
            stream.Seek(512 * 3 * 2, SeekOrigin.Current); //location of $Volume
            MFTStartPos = stream.Position;

            MftEntry mftEntry = new MftEntry(stream);

            stream.Position = MFTStartPos;
            stream.Position += mftEntry.Header.OffsetToFirstAttribute; //location of First Attribute 

            AttrStartPos = stream.Position;

            while (true)
            {
                stream.Position = AttrStartPos;
                AttrType = stream.ReadUInt32();
                AttrLen = stream.ReadUInt32();

                if (AttrType == 96)
                {
                    stream.Position = AttrStartPos + 16;
                    SizeOfCont = stream.ReadInt32();
                    OffsetOfCont = stream.ReadUInt16();
                    stream.Position = AttrStartPos + OffsetOfCont;
                    volumeLable = stream.ReadString(SizeOfCont);
                    break;
                }
                else
                {
                    AttrStartPos += AttrLen;
                }
            }

        }
    }
}
