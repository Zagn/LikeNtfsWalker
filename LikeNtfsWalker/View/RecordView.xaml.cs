using LikeNtfsWalker.Model;
using LikeNtfsWalker.ViewModel;
using System.ComponentModel.Design;
using System.Windows.Controls;

namespace LikeNtfsWalker.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RecordView : UserControl
    {
        public RecordView(Partition scan)
        {
            InitializeComponent();
            DataContext = new RecordViewModel(scan);
        }
    }
}
