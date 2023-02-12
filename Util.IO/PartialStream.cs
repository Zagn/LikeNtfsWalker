using System;
using System.Collections.Generic;
using System.IO;

namespace Util.IO
{
    public class PartialStream : Stream
    {
        private List<Extent> Extents;

        public Stream stream;

        public PartialStream(Stream stream)
        {
            this.stream = stream;
            Extents = new List<Extent>();
        }

        private long length;
        public override long Length => length;

        public override long Position { get; set; }

        public void AddExtent(Extent extent)
        {
            Extents.Add(extent);
            length += extent.Size;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalSize = 0;
            var (currentIdx, realPos) = getCurrentExtent2(Position);

            int i = currentIdx;
            while (true)
            {
                var remainSize = count - totalSize;
                var canReadSize = (int)(Extents[i].Start + Extents[i].Size - realPos);
                var toReadSize = Math.Min(remainSize, canReadSize);

                stream.Seek(realPos, SeekOrigin.Begin);
                var readSize = stream.Read(buffer, offset, toReadSize);
                totalSize += readSize;

                if (readSize != toReadSize || totalSize == count)
                    break;

                offset += readSize;

                if (i < Extents.Count - 1)
                    realPos = Extents[i + 1].Start;
                i++;
            }
            Seek(totalSize, SeekOrigin.Current);  

            return totalSize;
        }

        private Tuple<int, long> getCurrentExtent2(long pos)
        {
            var prevSize = 0L;

            for (var i = 0; i < Extents.Count; i++)
            {
                if (pos < prevSize + Extents[i].Size)
                    return Tuple.Create(i, Extents[i].Start + (pos - prevSize));

                prevSize += Extents[i].Size;
            }

            throw new ArgumentOutOfRangeException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                Position = offset;
            }
            else if (origin == SeekOrigin.Current)
            {
                Position += offset;
            }
            else if (origin == SeekOrigin.End)
            {
                Position = Length + offset;
            }
            return Position;
        }

        #region Not implement
        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            stream.Close();
        }
        #endregion
    }
}
