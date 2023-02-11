using LikeNtfsWalker.UI;
using System.IO;

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
        private string size;

        public string Size
        {
            get => size;
            set
            {
                size = value;
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

        // Property
        private string property;

        public string Property
        {
            get => property;
            set
            {
                property = value;
                RaisePropertyChanged();
            }
        }

        // Description
        private string description;

        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged();
            }
        }

        public Stream DataStream { get; set; }

        public Record(string id, string filename, string size, string cdate, string mdate, string attributes, string property, string description)
        {
            this.id = id;
            this.filename = filename;
            this.size= size;
            this.cdate = cdate;
            this.mdate = mdate;
            this.attributes = attributes;
            this.property = property;
            this.description = description;
        }
    }
}
