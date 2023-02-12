using LikeNtfsWalker.Model;
using LikeNtfsWalker.UI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;

namespace LikeNtfsWalker.ViewModel
{
    public class SelectDiskViewModel : Notifier
    {
        private Disk selectedDisk;

        public Disk SelectedDisk
        {
            get => selectedDisk;
            set
            {
                selectedDisk = value;
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
                SelectedDisk = null;
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

            RefreshCommand = new Command(ExecuteRefreshCommand);
            BrowseImageCommand = new Command(ExecuteBrowsImageCommand);
        }

        public void ExecuteRefreshCommand(object parameter)
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

        public void ExecuteBrowsImageCommand(object parameter)
        {
            var dialog = new OpenFileDialog();

            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedDisk = new Disk("0", "0", "0", "0", dialog.FileName);
            }
            else
            {
                return;
            }
        }
     }
}