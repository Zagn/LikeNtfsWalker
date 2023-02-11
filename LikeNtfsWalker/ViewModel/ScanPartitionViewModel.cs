using Filesystem.Partition;
using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using Filesystem.Ntfs;
using System.Collections.Generic;

namespace LikeNtfsWalker.ViewModel
{
    public class ScanPartitionViewModel : Notifier
    {
        private Scan newparttition;

        public Scan NewParttition
        {
            get => newparttition;
            set
            {
                newparttition = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Scan> ScanList { get; set; }


        public ScanPartitionViewModel(Model.Disk disk) 
        { 
            ScanList = new ObservableCollection<Scan>();

            Mbr mbr = new Mbr(disk.FilePath);

            if (mbr.signature != 21930) //Signature Value
            {
                ScanList.Add(new Scan("There is no MBR"));
            }
            else
            {
                foreach (var partition in mbr.partitions)
                {
                    //ScanList.Add(new Scan(partition.PartitionType.ToString()));
                }

            }
        }
    }
}
