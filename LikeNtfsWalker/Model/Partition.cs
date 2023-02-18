using Filesystem.Ntfs;
using LikeNtfsWalker.UI;
using System.Windows.Media.Animation;
using Util.IO;

namespace LikeNtfsWalker.Model
{
    public class Partition : Notifier
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private string fileSystem;

        public string FileSystem
        {
            get => fileSystem;
            set
            {
                fileSystem = value;
                RaisePropertyChanged();
            }
        }

        private string filePath;

        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                RaisePropertyChanged();
            }
        }

        private long partitionStartOffset;

        public long PartitionStartOffsets
        {
            get => partitionStartOffset;
            set
            {
                partitionStartOffset = value;
                RaisePropertyChanged();
            }
        }

        private long partitionEndOffset;

        public long PartitionEndOffset
        {
            get => partitionEndOffset;
            set
            {
                partitionEndOffset = value;
                RaisePropertyChanged();
            }
        }

        private int bytePerSector;

        public int BytePerSector
        {
            get => bytePerSector;
            set
            {
                bytePerSector = value;
                RaisePropertyChanged();
            }
        }

        public Partition(string name, string fileSystem, string filePath , long partitionStartOffset, long partitionEndOffset, int bytePerSector)
        {
            this.name = name;
            this.fileSystem = fileSystem;
            this.filePath = filePath;
            this.partitionStartOffset = partitionStartOffset;
            this.partitionEndOffset = partitionEndOffset;
            this.bytePerSector = bytePerSector;
        }
    }
}
