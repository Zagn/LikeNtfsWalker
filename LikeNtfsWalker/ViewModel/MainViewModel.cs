using LikeNtfsWalker.UI;
using LikeNtfsWalker.View;
using System.Collections.Generic;
using System.Windows.Controls;

using Application = System.Windows.Application;

namespace LikeNtfsWalker.ViewModel
{
    enum ViewState
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
            switch (state)
            {
                case ViewState.SelectDrive:
                    if(currentView.DataContext is SelectDiskViewModel selectDiskVM)
                    {
                        // 선택한 Disk가 MBR인지 확인
                        views.Push(CurrentView);
                        CurrentView = new ScanPartitionWindow(selectDiskVM.SelectDisk);
                        state = ViewState.SelectPartition;
                    }
                    break;

                case ViewState.SelectPartition:
                    if(currentView.DataContext is ScanPartitionViewModel)
                    {
                        views.Push(CurrentView);
                        CurrentView = new RecordView();
                        state = ViewState.ViewRecord;
                    }
                    break;
            }
        }

        private void GotoPrevious(object parameter)
        {
            switch (state)
            {
                case ViewState.SelectPartition:
                    CurrentView = views.Pop();
                    state = ViewState.SelectDrive;
                    break;
                case ViewState.ViewRecord:
                    CurrentView = views.Pop();
                    state = ViewState.SelectPartition;
                    break;
            }
        }

        private void Exit(object parameter)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
