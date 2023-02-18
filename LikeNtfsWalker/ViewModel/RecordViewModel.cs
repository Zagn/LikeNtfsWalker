using Filesystem.Ntfs;
using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using Util.IO;
using System;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Forms.Design;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace LikeNtfsWalker.ViewModel
{
    public class RecordViewModel : Notifier
    {
        string mftNumber = string.Empty;
        string name = string.Empty;
        string size = string.Empty;
        string dataCreated = string.Empty;
        string dataModified = string.Empty;
        string attribute = string.Empty;
        long mftCount = 0;

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
            
            
            var stream = new DeviceStream(partition.FilePath, partition.BytePerSector);
            var partialStream = new PartialStream(stream);

            Extent extent = new Extent(partition.PartitionStartOffsets, partition.PartitionEndOffset);
            
            partialStream.AddExtent(extent);
            var ntfsFileSystem = new NTFSFileSystem(partialStream);

            ntfsFileSystem.BuildFilesystem();

            foreach (var mftEntry in ntfsFileSystem.MftEntries)
            {
                mftNumber = Convert.ToString(mftCount++);

                var fileInfoList = new ObservableCollection<FileInfo>
                {
                    BaseInfo(mftEntry)
                };

                foreach (var mftAttr in mftEntry.Attributes)
                {
                    switch (mftAttr.Type)
                    {
                        case AttType.SIA:  
                            fileInfoList.Add(SatandardInfo(mftAttr));
                            break;
                        case AttType.FileName:
                            fileInfoList.Add(FileNameInfo(mftAttr));
                            break;
                        case AttType.Data:
                            fileInfoList.Add(DataInfo(mftAttr));
                            //HexData = mftEntry.DataStream.ReadBytes((uint)mftEntry.DataStream.Length);
                            break;
                        case AttType.Bitmap:
                            fileInfoList.Add(BitmapInfo(mftAttr));                                
                            break;
                        case AttType.VolumeName:
                            fileInfoList.Add(VolumeInfo(mftAttr));
                            break;
                        default:
                            break;
                    }
                }
                recordslist.Add(new MftRecord(mftNumber, name, size, dataCreated, dataModified, attribute, fileInfoList));
            }
        }
        public FileInfo BaseInfo(MftEntry mftEntry)
        {
            var baseDictionary = new Dictionary<string, string>
            {
                { "Sequence Number", Convert.ToString(mftEntry.Header.SequenceNumber) },
                { "LinkCount", Convert.ToString(mftEntry.Header.LinkCount) },
                { "Record Size", Convert.ToString(mftEntry.Header.UsedSizeofMFTEntry) },
                { "Record Allocated Size", Convert.ToString(mftEntry.Header.AllocatedSizeOfMFTEntry) },
                { "Log File Sequence Number", Convert.ToString(mftEntry.Header.LogFileSequenceNumber) }
            };

            return new FileInfo("<Base information>", baseDictionary);
        }

        public FileInfo SatandardInfo(MftAttribute mftAttr)
        {
            StandardInformation sia = (StandardInformation)mftAttr;

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

        public FileInfo FileNameInfo(MftAttribute mftAttr)
        {
            FileName fileName = (FileName)mftAttr;

            name = fileName.Name.Replace("\0", "");
            size = Convert.ToString(fileName.RealSizeOfFile / 1024) + "KB";
            dataCreated = Convert.ToString(fileName.CreationTime);
            dataModified = Convert.ToString(fileName.ModifiedTime);

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

        public FileInfo DataInfo(MftAttribute mftAttr)
        {
            DataAttribute dataAttribute = (DataAttribute)mftAttr;

            var dataDictionary = new Dictionary<string, string>()
            {
                {"Type ", Convert.ToString(dataAttribute.Type)}
            };
            
            //long count = 0; //Data Null 나옴

            //foreach (var i in dataAttribute.Data)
            //{
            //    HexData[count++] = i;
            //}



            return new FileInfo("<Attribute : Data (0x80)>", dataDictionary);
        }

        public FileInfo BitmapInfo(MftAttribute mftAttr)
        {
            Bitmap bitmap = (Bitmap)mftAttr;

            var bitmapDictionary = new Dictionary<string, string>()
            {
                {"Type  ", Convert.ToString(bitmap.Type)},
                {"BitFiled", Convert.ToString(bitmap.BitFiled)}
            };

            return new FileInfo("<Attribute : Bitmap (0xB0)>", bitmapDictionary);
        }


        public FileInfo VolumeInfo(MftAttribute mftAttr)
        {
            VolumeName volumeName = (VolumeName)mftAttr;

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
