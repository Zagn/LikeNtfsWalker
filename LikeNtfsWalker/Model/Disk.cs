using LikeNtfsWalker.UI;

namespace LikeNtfsWalker.Model
{
    public class Disk : Notifier
    {
        // 디스크 이름
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        // 디스크 사이즈
        private string size;

        public string Size
        {
            get => size;
            set
            {
                size = value;
                RaisePropertyChanged();
            }
        }

        public Disk(string name, string size)
        {
            this.name = name;
            this.size = size;
        }
    }
 }
