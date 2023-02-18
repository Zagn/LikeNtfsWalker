using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Xml.Linq;

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

        private byte[] hexData;

        public byte[] HexData
        {
            get => hexData;
            set
            {
                hexData = value;
                RaisePropertyChanged();
            }
        }

        public Command SaveCommand { get; set; }

        public RecordViewModel(Partition partition)
        {
            recordslist = new ObservableCollection<MftRecord>();
            SaveCommand = new Command(Savefile);
            hexData = new byte[] { 1, 2, 3, 4, 5 };
            partition.NtfsFileSystem.BuildFilesystem();
        }

        public void Savefile(object parameter)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.CheckPathExists = true;

            dialog.ShowDialog();
        }
    }
}
