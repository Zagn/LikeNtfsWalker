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

            string partitionType;

            if (mbr.signature != 21930) //Signature Value
            {
                ScanList.Add(new Scan("There is no MBR", ""));
            }
            else
            {
                foreach (var partition in mbr.partitions)
                {
                    partitionType = GetPartitionType(partition.PartitionType);
                    //ScanList.Add(new Scan(partitionType);
                }

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
