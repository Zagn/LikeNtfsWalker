using Filesystem.Ntfs;
using Filesystem.Partition;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
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
                var mbr = new Mbr(disk.FilePath);

                var stream = new DeviceStream(disk.FilePath, 512);

                var partialStream = new PartialStream(stream); //이거 변경해야함

                foreach (var partition in mbr.partitions)
                {
                    
                    Extent extent = new Extent((long)partition.StartingLBAAddr * 512, (long)partition.SizeInSector * 512);
                    partialStream.AddExtent(extent);

                    Partitions.Add(new Model.Partition(VolumeLable.FromNtfs(partialStream), GetPartitionType(partition.PartitionType), partialStream));

                }
            }
            catch
            {
                MessageBox.Show("Error Occur : \"ScanPartitionViewModel()\"");
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
