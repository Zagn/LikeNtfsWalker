using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace LikeNtfsWalker.ViewModel
{
    public class SelectDiskViewModel : Notifier
    {
        private Model.Disk newdisk;

        public Model.Disk NewDisk
        {
            get => newdisk;
            set
            {
                newdisk = value;
                RaisePropertyChanged();
            }
        }

        private Model.Disk selectdisk;

        public Model.Disk SelectDisk
        {
            get => selectdisk;
            set
            {
                selectdisk = value;
                RaisePropertyChanged();
            }
        }

        // 디스크 리스트
        private ObservableCollection<Model.Disk> diskList;
        public ObservableCollection<Model.Disk> DiskList
        {
            get => diskList;
            set
            {
                diskList = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Model.Disk> logicalDiskList;
        private ObservableCollection<Model.Disk> physicalDiskList;

        // refresh 버튼
        public Command RefreshCommand { get; set; }
        public Command SelectLogicalCommand { get; set; }
        public Command SelectPhysicalCommand { get; set; }
        public Command BrowseImageCommand { get; set; }

        public SelectDiskViewModel()
        {
            physicalDiskList = getPhysicalDiskList();
            if (physicalDiskList == null)
                Debug.WriteLine("Error occur \"getPhysicalDiskList()\"");

            //logicalDiskList = getLogicalDiskList();
            //if (logicalDiskList == null)
            //    Debug.WriteLine("Error occur \"getLogicalDiskList()\"");

            diskList = physicalDiskList;
            RefreshCommand = new Command(Refresh);
            SelectLogicalCommand = new Command(SelectLogical);
            SelectPhysicalCommand = new Command(SelectPhysical);
            BrowseImageCommand = new Command(BrowsImage);
        }

        public void Refresh(object parameter)
        {
            physicalDiskList.Clear();
            //logicalDiskList.Clear();

            physicalDiskList = getPhysicalDiskList();
            if (physicalDiskList == null)
                Debug.WriteLine("Error occur \"getPhysicalDiskList()\"");

            //logicalDiskList = getLogicalDiskList();
            //if (logicalDiskList == null)
            //    Debug.WriteLine("Error occur \"getLogicalDiskList()\""); //Logical 없애깅
        }

        private ObservableCollection<Model.Disk> getPhysicalDiskList()
        {
            try
            {
                var disks = new ObservableCollection<Model.Disk>();
                ManagementObjectSearcher driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
                foreach (ManagementObject d in driveQuery.Get())
                {
                    Filesystem.Partition.Disk disk = new Filesystem.Partition.Disk(d);
                    disks.Add(new Model.Disk(disk.driveName + "\\ ", disk.diskModel + " ", Convert.ToString((disk.totalSpace / (1024 * 1024 * 1024)) + "GB"), disk.fileSystem + " ", disk.physicalName));
                }

                return disks;
            }
            catch
            {
                return null;
            }
        }

        //public ObservableCollection<Model.Disk> getLogicalDiskList()
        //{
        //    try
        //    {
        //        var disks = new ObservableCollection<Model.Disk>();
        //        DriveInfo[] allDrives = DriveInfo.GetDrives();
               
        //        foreach (DriveInfo d in allDrives)
        //        {
        //            disks.Add(new Model.Disk(d.Name + " ", "-Logical- ", Convert.ToString((d.TotalSize / (1024 * 1024 * 1024)) + "GB"), d.DriveFormat + " "));
                    
        //        }
        //        return disks;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public void BrowsImage(object parameter)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.CheckFileExists= true;
            dialog.CheckPathExists= true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectDisk = new Disk("0","0","0","0", dialog.FileName);
            }
            else
            {
                return;
            }
        }


        public void SelectLogical(object parameter)
        {
            DiskList = logicalDiskList;
        }

        public void SelectPhysical(object parameter)
        {
            DiskList = physicalDiskList;
        }
    }
}