using LikeNtfsWalker.UI;

namespace LikeNtfsWalker.Model
{
    public class Record : Notifier
    {
        // MFT#
        private string id;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged();
            }
        }

        // 파일 이름
        private string filename;

        public string FileName
        {
            get => filename;
            set
            {
                filename = value;
                RaisePropertyChanged();
            }
        }

        // 상태
        private string state;

        public string State
        {
            get => state;
            set
            {
                state = value;
                RaisePropertyChanged();
            }
        }

        // DateCreate
        private string cdate;

        public string CDate
        {
            get => cdate;
            set
            {
                cdate = value;
                RaisePropertyChanged();
            }
        }

        // DateModified
        private string mdate;

        public string MDate
        {
            get => mdate;
            set
            {
                mdate = value;
                RaisePropertyChanged();
            }
        }

        // Atribute
        private string attributes;

        public string Attributes
        {
            get => attributes;
            set
            {
                attributes = value;
                RaisePropertyChanged();
            }
        }
    }
}
