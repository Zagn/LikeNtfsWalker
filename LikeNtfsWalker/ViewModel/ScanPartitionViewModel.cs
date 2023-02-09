using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;

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


        public ScanPartitionViewModel(Disk disk) 
        { 
            ScanList = new ObservableCollection<Scan>();
        }
    }
}
