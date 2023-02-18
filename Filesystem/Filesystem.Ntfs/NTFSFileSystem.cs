using System.Collections.Generic;
using System.IO;
using Util.IO;

namespace Filesystem.Ntfs
{
    public class NTFSFileSystem
    {
        public Stream Stream;
        public VBR vbr;
        public MFTEntryHeader mfth;

        public List<MftEntry> MftEntries;

        public NTFSFileSystem(Stream stream)
        {
            Stream = stream;
            MftEntries = new List<MftEntry>();
            vbr = new VBR(Stream);
        }

        public void BuildFilesystem()
        {
            Stream.Seek((long)vbr.MftStartOffset, SeekOrigin.Begin);

            var mft = new MftEntry(Stream, vbr.ClusterSize); // $MFT
            var mftStream = mft.DataStream as PartialStream;
            if (mftStream == null)
                return;

            while (mftStream.Position < mftStream.Length) //mft.DataStram is null
            {
                Stream.Seek(mftStream.GetRealPosition(), SeekOrigin.Begin);
                MftEntries.Add(new MftEntry(Stream, vbr.ClusterSize));

                mftStream.Seek(0x400, SeekOrigin.Current);
            }
        }

        ~NTFSFileSystem()
        {
        }

        
    }
}
