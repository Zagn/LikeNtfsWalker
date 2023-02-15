using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;

namespace LikeNtfsWalker.ViewModel
{
    public class RecordViewModel : Notifier
    {
        private ObservableCollection<MftRecord> recordslist;

        public ObservableCollection<MftRecord> RecordList
        {
            get => recordslist;
            set
            {
                recordslist = value;
                RaisePropertyChanged();
            }
        }

        private MftRecord selectedrecord;

        public MftRecord SelectedRecord
        {
            get => selectedrecord;
            set
            {
                selectedrecord = value;
                RaisePropertyChanged();
            }
        }

        public Command SaveCommand { get; set; }

        public RecordViewModel(Partition partition)
        {
            recordslist = new ObservableCollection<MftRecord>();
            SaveCommand = new Command(Savefile);
   
        }

        public void Savefile(object parameter)
        {
            // 선택한 정보를 저장
            var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.CheckPathExists = true;

            dialog.ShowDialog();
        }

    }
}
