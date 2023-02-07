using LikeNtfsWalker.ViewModel;
using System.Windows.Controls;

namespace LikeNtfsWalker.View
{
    /// <summary>
    /// SelectDiskWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectDiskWindow : UserControl
    {
        public SelectDiskWindow()
        {
            InitializeComponent();
            DataContext = new SelectDiskViewModel();
        }
    }
}
