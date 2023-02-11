using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Util.IO;

namespace Filesystem.Ntfs
{
    public class VolumeLable
    {
        public static string FromNtfs(Stream stream)
        {
            stream.Seek(512 * 3 * 2, SeekOrigin.Current); //location of $Volume
            var mftStartPos = stream.Position;

            MftEntry mftEntry = new MftEntry(stream);

            stream.Position = mftStartPos;
            stream.Position += mftEntry.Header.OffsetToFirstAttribute; //location of First Attribute 

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

                    return stream.ReadString(sizeOfCont);
                }
                else
                {
                    attrStartPos += attrLen;
                }
            }
            return null;
        }
    }
}

