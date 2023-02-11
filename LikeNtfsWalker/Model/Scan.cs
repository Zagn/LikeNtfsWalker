using LikeNtfsWalker.UI;

namespace LikeNtfsWalker.Model
{
    public class Scan : Notifier
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

        public Scan(string name, string fileSystem)
        {
            this.name = name;
            this.fileSystem = fileSystem;
        }
    }
}
