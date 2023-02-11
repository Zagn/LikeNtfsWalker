using LikeNtfsWalker.Model;
using LikeNtfsWalker.ViewModel;
using System.Windows.Controls;

namespace LikeNtfsWalker.View
{
    /// <summary>
    /// ScanPartitionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScanPartitionWindow : UserControl
    {
        public ScanPartitionWindow(Disk disk)
        {
            InitializeComponent();
            DataContext = new ScanPartitionViewModel(disk);
        }
    }
}