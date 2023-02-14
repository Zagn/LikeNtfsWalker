using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Util.IO;

namespace Filesystem.Ntfs
{
    internal class MftEntry
    {
        public MFTEntryHeader Header { get; }
        public Dictionary<AttType, MftAttribute> Attributes = new Dictionary<AttType, MftAttribute>();
        public Stream DataStream { get; }
        

        public MftEntry(Stream stream, int clusterSize)
        {
            var mftentrystartpos = stream.Position;
            var endmarkersize = 8;

            Header = new MFTEntryHeader(stream);
            var MFTEntrySize = Header.UsedSizeofMFTEntry + mftentrystartpos;

            var FixupArraySignature = 2;
            var FixupArray = 2;
            stream.Seek(FixupArraySignature + Header.CountOfFixupValues * FixupArray , SeekOrigin.Current);

            var pos = stream.Position;
            while (pos < MFTEntrySize - endmarkersize)
            {
                // Non-Resident Flag 값을 먼저 확인
                // 그에 따라 NonResidentHeader or ResidentHeader 생성
                // ResidentHeader는 ClusterRun 또한 분석 후 보관

                stream.Seek(8, SeekOrigin.Current);
                var isNonResident = stream.ReadBool();
                var attOffset = stream.Seek(-9, SeekOrigin.Current);
                var header = isNonResident ? (MftAttHeader)new NonResidentHeader(stream) : new ResidentHeader(stream);
                var attDataStream = isNonResident ? clusterRunToExtents(((NonResidentHeader)header).ClusterRuns, stream, clusterSize) : stream;

                switch (header.Type)
                {
                    case AttType.SIA:
                        Attributes.Add(AttType.SIA, new StandardInformation(header, attDataStream));
                        break;
                    case AttType.FileName:
                        Attributes.Add(AttType.FileName, new FileName(header, attDataStream));
                        break;
                    case AttType.Data:
                        DataStream = attDataStream;
                        Attributes.Add(AttType.Data, new DataAttribute(header, attDataStream));
                        break;
                    case AttType.Bitmap:
                        Attributes.Add(AttType.Bitmap, new Bitmap(header, attDataStream));
                        break;
                    case AttType.VolumeName:
                        Attributes.Add(AttType.VolumeName, new VolumeName(header, attDataStream));
                        break;
                    default:
                        stream.Seek(attOffset + header.AttLength, SeekOrigin.Begin);
                        break;
                }
                pos = attOffset;
            }

            var paddingMftentry = 1024 - Header.UsedSizeofMFTEntry + endmarkersize;
            stream.Seek(paddingMftentry, SeekOrigin.Current);
        }
        
        

        public PartialStream clusterRunToExtents(List<ClusterRun> clusterRun, Stream stream, int clusterSize)
        {
            var dataStream = new PartialStream(stream);
            var padding = 0L;

            foreach (var cluster in clusterRun)
            {
                var startOffset = cluster.RunOffset * clusterSize;
                var length = (long)cluster.RunLength * clusterSize;
                dataStream.AddExtent(new Extent(startOffset + padding, length));

                padding = startOffset;
            }
            
            return dataStream;
            
        }
    }
}
