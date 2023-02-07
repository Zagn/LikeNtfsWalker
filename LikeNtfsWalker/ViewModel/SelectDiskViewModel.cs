using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;

namespace LikeNtfsWalker.ViewModel
{
    public class SelectDiskViewModel : Notifier
    {
        private Disk newdisk;

        public Disk NewDisk
        {
            get => newdisk;
            set
            {
                newdisk = value;
                RaisePropertyChanged();
            }
        }

        private Disk selectdisk;

        public Disk SelectDisk
        {
            get => selectdisk;
            set
            {
                selectdisk = value;
                RaisePropertyChanged();
            }
        }

        // 디스크 리스트
        private ObservableCollection<Disk> diskList;
        public ObservableCollection<Disk> DiskList
        {
            get => diskList;
            set
            {
                diskList= value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Disk> logicalDiskList;
        private ObservableCollection<Disk> physicalDiskList;

        // refresh 버튼
        public Command RefreshCommand { get; set; }
        public Command SelectLogicalCommand { get; set; }
        public Command SelectPhysicalCommand { get; set; }

        public SelectDiskViewModel()
        {
            logicalDiskList = new ObservableCollection<Disk>();
            physicalDiskList = new ObservableCollection<Disk>();
            diskList = physicalDiskList;
            RefreshCommand = new Command(Refresh);
            SelectLogicalCommand = new Command(SelectLogical);
            SelectPhysicalCommand = new Command(SelectPhysical);
        }

        public void Refresh(object parameter)
        {

        }

        public void SelectLogical(object parameter)
        {
            DiskList = logicalDiskList;
        }

        public void SelectPhysical(object parameter)
        {
            DiskList = physicalDiskList;
        }
    }
}