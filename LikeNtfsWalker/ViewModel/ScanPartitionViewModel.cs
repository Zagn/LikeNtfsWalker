using Filesystem.Partition;
using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using Filesystem.Ntfs;
using System.Collections.Generic;
using System.Windows;

namespace LikeNtfsWalker.ViewModel
{
    public class ScanPartitionViewModel : Notifier
    {
        private Scan selectparttition;

        public Scan SelectParttition
        {
            get => selectparttition;
            set
            {
                selectparttition = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Scan> ScanList { get; set; }


        public ScanPartitionViewModel(Model.Disk disk) 
        { 
            ScanList = new ObservableCollection<Scan>();

            //mbr인지 확인문
            try
            {
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
            catch
            {
                MessageBox.Show("파티션 테이블이 MBR이 아닙니다.");
            }
        }
    }
}
