using LikeNtfsWalker.UI;
using LikeNtfsWalker.View;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

        // 다음 버튼
        public Command GotoNextCommand { get; set; }
        // 이전 버튼
        public Command GotoPrevCommand { get; set; }
        // 나가기 버튼
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

        // 다음 버튼 이벤트
        private void GotoNext(object parameter)
        {
            switch (state)
            {
                case ViewState.SelectDrive:
                    views.Push(CurrentView);
                    CurrentView = new ScanPartitionWindow();
                    state = ViewState.SelectPartition;
                    break;

                case ViewState.SelectPartition:
                    views.Push(CurrentView);
                    CurrentView = new RecordView();
                    state = ViewState.ViewRecord;
                    break;
            }
        }

        // 이전 버튼 이벤트
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

        //  나가기 버튼 이벤트
        private void Exit(object parameter)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
