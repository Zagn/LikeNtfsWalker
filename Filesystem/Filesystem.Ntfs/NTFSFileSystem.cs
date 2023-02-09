﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void BuildFilesystem()
        {
            vbr = new VBR(Stream);
            mfth = new MFTEntryHeader(Stream);
        }

        ~NTFSFileSystem()
        {
            Stream.Close();
        }
    }
}