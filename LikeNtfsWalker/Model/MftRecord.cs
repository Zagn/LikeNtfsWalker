using LikeNtfsWalker.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace LikeNtfsWalker.Model
{
    public class MftRecord : Notifier
    {
        private uint mftNumber;

        public uint MftNumber
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

        private DateTime? createDate;

        public DateTime? CreateDate
        {
            get => createDate;
            set
            {
                createDate = value;
                RaisePropertyChanged();
            }
        }

        // DateModified
        private DateTime? modifiedDate;

        public DateTime? ModifiedDate
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

        private List<FileInfo> fileInfoList;

        public List<FileInfo> FileInfoList
        {
            get => fileInfoList;
            set
            {
                fileInfoList = value;
                RaisePropertyChanged();
            }
        }

        public Stream DataStream { get; set; }

        public MftRecord(uint mftNumber, string fileName, string size, DateTime? created, DateTime? modified, string attributes, List<FileInfo> fileInfoList)
        {
            this.mftNumber = mftNumber;
            this.fileName = fileName;
            this.size= size;
            createDate = created;
            modifiedDate = modified;
            this.attributes = attributes;
            this.fileInfoList = fileInfoList;
        }
    }
}
