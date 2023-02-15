using System.Collections.Generic;
using System.IO;

namespace Filesystem.Ntfs
{
    public class NTFSFileSystem
    {
        public Stream Stream;
        public VBR vbr;
        public MFTEntryHeader mfth;

        internal List<MftEntry> MftEntries;

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
            var padding = 0L;

            while (mft.DataStream.Position < mft.DataStream.Length) //mft.DataStram is null
            {

                MftEntries.Add(new MftEntry(mft.DataStream, vbr.ClusterSize));
               
                if (mft.DataStream.Position % 4 != 0)
                    padding = 4 - mft.DataStream.Position % 4;
                mft.DataStream.Seek(padding, SeekOrigin.Current);
            }
        }

        ~NTFSFileSystem()
        {
            Stream.Close();
        }
    }
}
