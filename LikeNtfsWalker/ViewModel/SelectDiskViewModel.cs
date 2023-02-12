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
        private Disk selectdisk;

        public Disk SelectDisk
        {
            get => selectdisk;
            set
            {
                selectdisk = value;
                RaisePropertyChanged();
            }
        }

        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                selectedTabIndex = value;
                SelectDisk = null;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Disk> diskList;
        
        public ObservableCollection<Disk> DiskList
        {
            get => diskList;
            set
            {
                diskList = value;
                RaisePropertyChanged();
            }
        }

        public Command RefreshCommand { get; set; }
       
        public Command BrowseImageCommand { get; set; }

        public SelectDiskViewModel()
        {
            diskList = getPhysicalDiskList();
            if (diskList == null)
                Debug.WriteLine("Error occur \"getPhysicalDiskList()\"");

            RefreshCommand = new Command(Refresh);
            BrowseImageCommand = new Command(BrowsImage);
        }

        public void Refresh(object parameter)
        {
            DiskList.Clear();
          
            DiskList = getPhysicalDiskList();
            if (DiskList == null)
                Debug.WriteLine("Error occur \"getPhysicalDiskList()\"");
        }

        private ObservableCollection<Disk> getPhysicalDiskList()
        {
            try
            {
                var disks = new ObservableCollection<Disk>();
                ManagementObjectSearcher driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
                foreach (ManagementObject d in driveQuery.Get())
                {
                    Filesystem.Partition.Disk disk = new Filesystem.Partition.Disk(d);
                    disks.Add(new Disk(disk.driveName + "\\ ", disk.diskModel + " ", Convert.ToString(Math.Round((double)disk.totalSpace / (1024 * 1024 * 1024), 1)) + "GB", disk.fileSystem + " ", disk.physicalName));
                }

                return disks;
            }
            catch
            {
                return null;
            }
        }

        public void BrowsImage(object parameter)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectDisk = new Disk("0", "0", "0", "0", dialog.FileName);
            }
            else
            {
                return;
            }
        }
     }
}