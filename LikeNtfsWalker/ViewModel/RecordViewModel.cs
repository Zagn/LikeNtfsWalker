using Filesystem.Ntfs;
using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Util.IO;


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
            init();

            var entries = buildNtfs(partition);
            foreach (var mftEntry in entries)
                recordslist.Add(MakeMftRecord(mftEntry));
        }

        private void init()
        {
            recordslist = new ObservableCollection<MftRecord>();
            SaveCommand = new Command(Savefile);
        }

        private List<MftEntry> buildNtfs(Partition partition)
        {
            var stream = new DeviceStream(partition.FilePath, partition.BytePerSector);
            var partialStream = new PartialStream(stream);
            partialStream.AddExtent(new Extent(partition.PartitionStartOffsets, partition.PartitionEndOffset));
            var ntfsFileSystem = new NTFSFileSystem(partialStream);

            ntfsFileSystem.BuildFilesystem();

            return ntfsFileSystem.MftEntries;
        }

        private MftRecord MakeMftRecord(MftEntry entry)
        {
            var name = string.Empty;
            var size = string.Empty;
            DateTime? dataCreated = null;
            DateTime? dataModified = null;
            var attribute = string.Empty;
            var fileInfoList = new List<FileInfo>() { MakeFileInfo(entry.Header) };

            foreach (var mftAttr in entry.Attributes)
            {
                switch (mftAttr.Type)
                {
                    case AttType.SIA:
                        fileInfoList.Add(MakeFileInfo((StandardInformation)mftAttr));
                        break;

                    case AttType.FileName:
                        var fileName = (FileName)mftAttr; ;
                        name = fileName.Name.Replace("\0", "");
                        size = Convert.ToString(fileName.RealSizeOfFile / 1024) + "KB";
                        dataCreated = DateTime.FromFileTime((long)fileName.CreationTime);
                        dataModified = DateTime.FromFileTime((long)fileName.ModifiedTime);
                        fileInfoList.Add(MakeFileInfo(fileName));
                        break;

                    case AttType.Data:
                        fileInfoList.Add(MakeFileInfo((DataAttribute)mftAttr));
                        break;

                    case AttType.Bitmap:
                        fileInfoList.Add(MakeFileInfo((Bitmap)mftAttr));
                        break;

                    case AttType.VolumeName:
                        fileInfoList.Add(MakeFileInfo((VolumeName)mftAttr));
                        break;

                    default:
                        break;
                }
            }

            return new MftRecord(entry.Header.NumberOfThisMFTEntry, name, size, dataCreated, dataModified, attribute, fileInfoList, entry.DataStream);
        }

        public FileInfo MakeFileInfo(MFTEntryHeader header)
        {
            var baseDictionary = new Dictionary<string, string>
            {
                { "Sequence Number", Convert.ToString(header.SequenceNumber) },
                { "LinkCount", Convert.ToString(header.LinkCount) },
                { "Record Size", Convert.ToString(header.UsedSizeofMFTEntry) },
                { "Record Allocated Size", Convert.ToString(header.AllocatedSizeOfMFTEntry) },
                { "Log File Sequence Number", Convert.ToString(header.LogFileSequenceNumber) }
            };

            return new FileInfo("<Base information>", baseDictionary);
        }

        public FileInfo MakeFileInfo(StandardInformation sia)
        {
            var siaDictionary = new Dictionary<string, string>
            {
                {"CreationTime", Convert.ToString(DateTime.FromFileTime((long)sia.CreationTime))},
                {"ModifiedTime", Convert.ToString(DateTime.FromFileTime((long)sia.ModifiedTime))},
                {"MFTModifiedTime", Convert.ToString(DateTime.FromFileTime((long) sia.MFTModifiedTime))},
                {"LastAccessedTime", Convert.ToString(DateTime.FromFileTime((long) sia.LastAccessedTime))},
                {"Flags", Convert.ToString(sia.Flags)},
                {"MaximumNumberOfVersions", Convert.ToString(sia.MaximumNumberOfVersions)},
                {"ClassID", Convert.ToString(sia.ClassID)},
                {"SecurityID", Convert.ToString(sia.SecurityID)},
                {"UpdateSequenceNumber", Convert.ToString(sia.UpdateSequenceNumber)}
            };
            
            return new FileInfo("<Attribute : Standart Information (0x10)>", siaDictionary);
        }

        public FileInfo MakeFileInfo(FileName fileName)
        {
            var fileNameDictionary = new Dictionary<string, string>
            {
                {"FileReferenceOfParentDirectory", Convert.ToString(fileName.FileReferenceOfParentDirectory)},
                {"CreationTime ", Convert.ToString(DateTime.FromFileTime((long)fileName.CreationTime))},
                {"ModifiedTime ", Convert.ToString(DateTime.FromFileTime((long)fileName.ModifiedTime))},
                {"MFTModifiedTime ", Convert.ToString(DateTime.FromFileTime((long) fileName.MFTModifiedTime))},
                {"LastAccessedTime ", Convert.ToString(DateTime.FromFileTime((long) fileName.LastAccessedTime))},
                {"AllocatedSizeOfFile", Convert.ToString(fileName.AllocatedSizeOfFile)},
                {"RealSizeOfFile", Convert.ToString(fileName.RealSizeOfFile)},
                {"Flags ", Convert.ToString(fileName.Flags)},
                {"ReparseValue", Convert.ToString(fileName.ReparseValue)},
                {"Namespace", Convert.ToString(fileName.Namespace).Replace("\0", "")},
                {"Name", Convert.ToString(fileName.Name).Replace("\0", "")}
            };
           
            return new FileInfo("<Attribute : File Name (0x30)>", fileNameDictionary);
        }

        public FileInfo MakeFileInfo(DataAttribute dataAttribute)
        {
            var dataDictionary = new Dictionary<string, string>()
            {
                {"Type ", Convert.ToString(dataAttribute.Type)}
            };
            
            return new FileInfo("<Attribute : Data (0x80)>", dataDictionary);
        }

        public FileInfo MakeFileInfo(Bitmap bitmap)
        {
            var bitmapDictionary = new Dictionary<string, string>()
            {
                {"Type  ", Convert.ToString(bitmap.Type)},
                {"BitFiled", Convert.ToString(bitmap.BitFiled)}
            };

            return new FileInfo("<Attribute : Bitmap (0xB0)>", bitmapDictionary);
        }


        public FileInfo MakeFileInfo(VolumeName volumeName)
        {
            var volumeDictionary = new Dictionary<string, string>()
            {
                {"UnicodeName", Convert.ToString(volumeName.UnicodeName)}
            };
            
            return new FileInfo("<Attribute : Volume (0x60)>", volumeDictionary);
        }

        public void Savefile(object parameter)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.CheckPathExists = true;

            dialog.ShowDialog();
        }
    }
}
