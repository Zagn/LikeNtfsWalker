using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System.Collections.ObjectModel;
using Filesystem.Partition;
using System.Collections.Generic;
using System.Management;
using System;
using System.IO;
using System.Diagnostics;

namespace LikeNtfsWalker.ViewModel
{
    public class SelectDiskViewModel : Notifier
    {
        private LikeNtfsWalker.Model.Disk newdisk;

        public LikeNtfsWalker.Model.Disk NewDisk
        {
            get => newdisk;
            set
            {
                newdisk = value;
                RaisePropertyChanged();
            }
        }

        private LikeNtfsWalker.Model.Disk selectdisk;

        public LikeNtfsWalker.Model.Disk SelectDisk
        {
            get => selectdisk;
            set
            {
                selectdisk = value;
                RaisePropertyChanged();
            }
        }

        // 디스크 리스트
        private ObservableCollection<LikeNtfsWalker.Model.Disk> diskList;
        public ObservableCollection<LikeNtfsWalker.Model.Disk> DiskList
        {
            get => diskList;
            set
            {
                diskList= value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<LikeNtfsWalker.Model.Disk> logicalDiskList;
        private ObservableCollection<LikeNtfsWalker.Model.Disk> physicalDiskList;

        // refresh 버튼
        public Command RefreshCommand { get; set; }
        public Command SelectLogicalCommand { get; set; }
        public Command SelectPhysicalCommand { get; set; }

        public SelectDiskViewModel()
        {
            logicalDiskList = new ObservableCollection<LikeNtfsWalker.Model.Disk>();
            physicalDiskList = new ObservableCollection<LikeNtfsWalker.Model.Disk>();
            diskList = physicalDiskList;
            RefreshCommand = new Command(Refresh);
            SelectLogicalCommand = new Command(SelectLogical);
            SelectPhysicalCommand = new Command(SelectPhysical);

            ManagementObjectSearcher driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                Filesystem.Partition.Disk disk = new Filesystem.Partition.Disk(d);
                physicalDiskList.Add(new LikeNtfsWalker.Model.Disk(disk.driveName + " ", disk.diskModel + " ", Convert.ToString((disk.totalSpace / (1024 * 1024 * 1024)) + "GB"), disk.fileSystem + " "));
            }

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                logicalDiskList.Add(new LikeNtfsWalker.Model.Disk(d.Name + " ", "-Logical- ", Convert.ToString((d.TotalSize / (1024 * 1024 * 1024)) + "GB"), d.DriveFormat + " "));
            }
        }

        public void Refresh(object parameter)
        {
            //logicalDiskList.Add(new Disk("logical", "123"));
            //physicalDiskList.Add(new Disk("physical", "567"));

            physicalDiskList.Clear();
            logicalDiskList.Clear();

            ManagementObjectSearcher driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                Filesystem.Partition.Disk disk = new Filesystem.Partition.Disk(d);
                physicalDiskList.Add(new LikeNtfsWalker.Model.Disk(disk.driveName + " ", disk.diskModel + " ", Convert.ToString((disk.totalSpace / (1024 * 1024 * 1024)) + "GB"), disk.fileSystem + " "));
            }

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                logicalDiskList.Add(new LikeNtfsWalker.Model.Disk(d.Name + " ", "-Logical- ", Convert.ToString((d.TotalSize / (1024 * 1024 * 1024)) + "GB"), d.DriveFormat + " "));
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