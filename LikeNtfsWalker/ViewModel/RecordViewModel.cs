using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;

namespace LikeNtfsWalker.ViewModel
{
    public class RecordViewModel : Notifier
    {
        private ObservableCollection<Record> recordslist;

        public ObservableCollection<Record> RecordList
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

        public RecordViewModel(Scan scan)
        {
            recordslist = new ObservableCollection<Record>();
            fileinfolist = new ObservableCollection<Record>();
            SaveCommand = new Command(Savefile);
        }

        public void Savefile(object parameter)
        {
            RecordList.Add(new Record("1", "2", "3", "4", "5", "6", "7", "8"));

            // 선택한 정보를 저장
           /* var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.CheckPathExists = true;

            dialog.ShowDialog();*/
        }
    }
}
