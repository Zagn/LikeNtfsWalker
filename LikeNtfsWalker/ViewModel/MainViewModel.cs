using Filesystem.Partition;
using LikeNtfsWalker.UI;
using LikeNtfsWalker.View;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Application = System.Windows.Application;

namespace LikeNtfsWalker.ViewModel
{
    public enum ViewState
    {
        SelectDrive,
        SelectPartition,
        ViewRecord
    }

    public class MainViewModel : Notifier
    {
        private UserControl currentView;
        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                RaisePropertyChanged();
            }
        }

        public Command GotoNextCommand { get; set; }

        public Command GotoPrevCommand { get; set; }

        public Command ExitCommand { get; set; }

        private Stack<UserControl> views;

        private ViewState state;

        public ViewState State
        {
            get => state;
            set
            {
                state = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            state = ViewState.SelectDrive;
            currentView = new SelectDiskWindow();
            views = new Stack<UserControl>();

            GotoNextCommand = new Command(GotoNext);
            GotoPrevCommand = new Command(GotoPrevious);
            ExitCommand = new Command(Exit);
        }

        private void GotoNext(object parameter)
        {
            switch (State)
            {
                case ViewState.SelectDrive:
                    if (currentView.DataContext is SelectDiskViewModel selectDiskVM)
                    {
                        if (selectDiskVM.SelectedDisk == null)
                            MessageBox.Show("디스크를 선택해주세요");
                        else if (IsMbr(selectDiskVM.SelectedDisk.FilePath) == false)
                        {
                            MessageBox.Show("파일 형식을 확인해주세요");
                        }
                        else
                        {
                            views.Push(CurrentView);
                            CurrentView = new ScanPartitionWindow(selectDiskVM.SelectedDisk);
                            State = ViewState.SelectPartition;
                        }
                    }
                    break;

                case ViewState.SelectPartition:
                    if (currentView.DataContext is ScanPartitionViewModel scanPartitionVM)
                    {
                        if (scanPartitionVM.SelectedPartition == null)
                            MessageBox.Show("스캔할 파티션을 선택해주세요");
                        else if (scanPartitionVM.SelectedPartition.FileSystem != "NTFS")
                        {
                            MessageBox.Show("NTFS 분석만을 지원하고 있습니다");
                        }
                        else
                        {
                            views.Push(CurrentView);
                            CurrentView = new RecordView(scanPartitionVM.SelectedPartition);
                            State = ViewState.ViewRecord;
                        }
                    }
                    break;
            }
        }

        private void GotoPrevious(object parameter)
        {
            switch (State)
            {
                case ViewState.SelectPartition:
                    CurrentView = views.Pop();
                    State = ViewState.SelectDrive;
                    break;
                case ViewState.ViewRecord:
                    CurrentView = views.Pop();
                    State = ViewState.SelectPartition;
                    break;
            }
        }

        private void Exit(object parameter)
        {
            Application.Current.MainWindow.Close();
        }

        private bool IsMbr(string filePath)
        {
            Mbr mbr = new Mbr(filePath);

            if (mbr.isMbr == true) return true; 
            else return false; 
        }
    }
}
