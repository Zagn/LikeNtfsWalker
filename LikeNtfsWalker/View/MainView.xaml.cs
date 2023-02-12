using LikeNtfsWalker.ViewModel;
using System.Windows;

namespace LikeNtfsWalker.View
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        // close 함수
        // viewmodel에서 함수 생성 후 알아서 하세요ㅎㅎㅎ
    }
}
