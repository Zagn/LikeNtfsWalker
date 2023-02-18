using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.IO;

namespace Filesystem.Ntfs
{
    internal class Program
    {
        public static void Main(string[] argv)
        {
            var path = "C://testtest.vhd";
            var padding = 0x10000;
            var baseStream = File.OpenRead(path);
            var stream = new PartialStream(baseStream);
            stream.AddExtent(new Extent(padding, baseStream.Length - padding));

            var ntfs = new NTFSFileSystem(stream);
            ntfs.BuildFilesystem();
        }
    }
}
