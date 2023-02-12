namespace Util.IO
{
    public class Extent
    {
        public long Start { get; set; }
        public long Size { get; set; }

        public Extent(long start, long size)
        {
            Start = start;
            Size = size;
        }
    }
}
