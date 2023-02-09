using LikeNtfsWalker.UI;

namespace LikeNtfsWalker.Model
{
    public class Disk : Notifier
    {
        // diskModel_ex : KXG60ZNV1T02 KIOXIA
        private string diskModel;

        public string DiskModel
        {
            get => diskModel;
            set
            {
                diskModel = value;
                RaisePropertyChanged();
            }
        }

        // 디스크 사이즈_totalSize
        private string size;

        public string Size
        {
            get => size;
            set
            {
                size = value;
                RaisePropertyChanged();
            }
        }

        //Disk FileSystem_ex : NTFS, FAT32, ...
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

        //DriveName_ex : F, C, E, ...
        private string driveName;
        public string DriveName
        {
            get => driveName;
            set
            {
                driveName = value;
                RaisePropertyChanged();
            }
        }


        public Disk(string driveName, string diskModel, string size, string fileSystem)
        {
            this.driveName = driveName;
            this.diskModel = diskModel;
            this.size = size;
            this.fileSystem = fileSystem;
        }
    }
 }
