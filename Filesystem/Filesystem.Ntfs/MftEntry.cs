using System.Collections.Generic;
using System.IO;
using Util.IO;

namespace Filesystem.Ntfs
{
    internal class MftEntry
    {
        public MFTEntryHeader Header { get; }
        public List<MftAttribute> Attributes { get; }

        public MftEntry(Stream stream)
        {
            Header = new MFTEntryHeader(stream);

            var pos = stream.Position;
            while (pos < Header.UsedSizeofMFTEntry)
            {
                // Non-Resident Flag 값을 먼저 확인
                // 그에 따라 NonResidentHeader or ResidentHeader 생성
                // ResidentHeader는 ClusterRun 또한 분석 후 보관

                stream.Seek(8, SeekOrigin.Current);
                var isNonResident = stream.ReadBool();
                var attOffset = stream.Seek(-9, SeekOrigin.Current);
                var header = isNonResident ? (MftAttHeader)new NonResidentHeader(stream) : new ResidentHeader(stream);

                var attDataStream = stream;
                if (isNonResident)
                {
                    stream.Seek(attOffset + header.AttLength, SeekOrigin.Begin);

                    return;
                    //var extents = clusterRunToExtents(((NonResidentHeader)header).ClusterRuns);
                    //attDataStream = new PartialStream(extents);
                }

                //var attDataStream = isNonResident ? new PartialStream(clusterRunToExtents(((NonResidentHeader)header).ClusterRuns)) : stream;

                switch (header.Type)
                {
                    case AttType.SIA:
                        Attributes.Add(new StandardInformation(header, attDataStream));
                        break;
                    case AttType.FileName:
                        Attributes.Add(new FileName(header, attDataStream));
                        break;
                    case AttType.Data:
                        Attributes.Add(new DataAttribute(header, attDataStream));
                        break;
                    case AttType.Bitmap:
                        Attributes.Add(new Bitmap(header, attDataStream));
                        break;
                    default:
                        stream.Seek(attOffset + header.AttLength, SeekOrigin.Begin);
                        break;
                }
            }
        }

        //public List<Extent> clusterRunToExtents(List<ClusterRun> clusterRun)
        //{

        //}
    }
}
