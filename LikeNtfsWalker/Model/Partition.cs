using Filesystem.Ntfs;
using LikeNtfsWalker.UI;
using Util.IO;

namespace LikeNtfsWalker.Model
{
    public class Partition : Notifier
    {
        // 파티션 이름
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

        private PartialStream partialStream;

        public PartialStream PartialStream
        {
            get => partialStream;
            set
            {
                partialStream = value;
                RaisePropertyChanged();
            }
        }

        public Partition(string name, string fileSystem, PartialStream partialStream)
        {
            this.name = name;
            this.fileSystem = fileSystem;
            this.partialStream = partialStream;
        }
    }
}
