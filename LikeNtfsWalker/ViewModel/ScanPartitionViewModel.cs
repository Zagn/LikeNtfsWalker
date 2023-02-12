using Filesystem.Ntfs;
using Filesystem.Partition;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using System.Windows;
using Util.IO;

namespace LikeNtfsWalker.ViewModel
{
    public class ScanPartitionViewModel : Notifier
    {
        private Model.Partition selectedParttition;

        public Model.Partition SelectedPartition
        {
            get => selectedParttition;
            set
            {
                selectedParttition = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Model.Partition> Partitions { get; set; }

        public ScanPartitionViewModel(Model.Disk disk)
        {
            Partitions = new ObservableCollection<Model.Partition>();
            
            try
            {
                Mbr mbr = new Mbr(disk.FilePath);
                DeviceStream stream = new DeviceStream(disk.FilePath, 512);

                if (mbr.signature != 21930) //Signature Value
                {
                    Partitions.Add(new Model.Partition("There is no MBR", "", null));
                }
                else
                {
                    foreach (var partition in mbr.partitions)
                    {
                        stream.Position = (long)partition.StartingLBAAddr * 512;
                        NTFSFileSystem ntfsFileSystem = new NTFSFileSystem(stream);
                        Partitions.Add(new Model.Partition(ntfsFileSystem.vbr.Lable, GetPartitionType(partition.PartitionType), ntfsFileSystem));
                    }
                }
            }
            catch
            {
                MessageBox.Show("파티션 테이블이 MBR이 아닙니다.");
            }
        }

        public string GetPartitionType(int type)
        {
            switch (type)
            {
                case 12:
                    return "FAT32";
                case 7:
                    return "NTFS";

                default:
                    return "";
            }
        }
    }
}
