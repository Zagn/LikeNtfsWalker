using LikeNtfsWalker.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LikeNtfsWalker.UI
{
    public class Notifier : INotifyPropertyChanged
    {
        private Disk newdisk;
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
