using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using System.IO;

namespace LikeNtfsWalker.Model
{
    public class MftRecord : Notifier
    {
        private string mftNumber;

        public string MftNumber
        {
            get => mftNumber;
            set
            {
                mftNumber = value;
                RaisePropertyChanged();
            }
        }

        private string fileName;

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                RaisePropertyChanged();
            }
        }

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

        private string createDate;

        public string CreateDate
        {
            get => createDate;
            set
            {
                createDate = value;
                RaisePropertyChanged();
            }
        }

        // DateModified
        private string modifiedDate;

        public string ModifiedDate
        {
            get => modifiedDate;
            set
            {
                modifiedDate = value;
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

        private ObservableCollection<FileInfo> fileInfoList;

        public ObservableCollection<FileInfo> FileInfoList
        {
            get => fileInfoList;
            set
            {
                fileInfoList = value;
                RaisePropertyChanged();
            }
        }

        public Stream DataStream { get; set; }

        public MftRecord(string mftNumber, string fileName, string size, string created, string modified, string attributes, ObservableCollection<FileInfo> fileInfoList)
        {
            this.mftNumber = mftNumber;
            this.fileName = fileName;
            this.size= size;
            this.createDate = created;
            this.modifiedDate = modified;
            this.attributes = attributes;
            this.fileInfoList = fileInfoList;
        }
    }
}
