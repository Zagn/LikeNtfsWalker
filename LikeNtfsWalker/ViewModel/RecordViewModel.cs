using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;

namespace LikeNtfsWalker.ViewModel
{
    public class RecordViewModel : Notifier
    {
        private ObservableCollection<Record> recordslist;

        public ObservableCollection<Record> REcordList
        {
            get => recordslist;
            set
            {
                recordslist = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Record> fileinfolist;

        public ObservableCollection<Record> Fileinfolist
        {
            get => fileinfolist;
            set
            {
                fileinfolist = value;
                RaisePropertyChanged();
            }
        }

        public Command SaveCommand { get; set; }

        public RecordViewModel()
        {
            recordslist = new ObservableCollection<Record>();
            fileinfolist = new ObservableCollection<Record>();
            SaveCommand = new Command(Savefile);
        }

        public void Savefile(object parameter)
        {
            // 스캔한 정보 파일로 저장

        }
    }
}
