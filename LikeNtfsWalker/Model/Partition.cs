using Filesystem.Ntfs;
using LikeNtfsWalker.UI;

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

        private NTFSFileSystem ntfsFileSystem;

        public NTFSFileSystem NtfsFileSystem
        {
            get => NtfsFileSystem;
            set
            {
                NtfsFileSystem = value;
                RaisePropertyChanged();
            }
        }

        public Partition(string name, string fileSystem, NTFSFileSystem ntfsFileSystem)
        {
            this.name = name;
            this.fileSystem = fileSystem;
            this.ntfsFileSystem = ntfsFileSystem;
        }
    }
}
