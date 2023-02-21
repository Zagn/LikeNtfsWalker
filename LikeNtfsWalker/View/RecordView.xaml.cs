using LikeNtfsWalker.Model;
using LikeNtfsWalker.ViewModel;
using System.ComponentModel.Design;
using System.Windows.Controls;
using Util.IO;

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

            //HexViewer.SetBytes();
        }

        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) //HexViewer
        //{
        //    if (e.AddedItems.Count == 0)
        //        return;

        //    var record = e.AddedItems[0] as MftRecord;
        //    if (record?.DataStream == null || record.DataStream.Length == 0)
        //    {
        //        //HexViewer.SetBytes(null);
        //        HexViewer.Refresh();
        //        return;
        //    }

        //    record.DataStream.Position = 0;

        //    //HexViewer.SetBytes(record.DataStream.ReadBytes((int)record.DataStream.Length)); 흐음 여기서 나오는데;;;
        //}
    }
}
