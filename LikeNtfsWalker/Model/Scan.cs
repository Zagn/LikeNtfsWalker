using LikeNtfsWalker.UI;

namespace LikeNtfsWalker.Model
{
    public class Scan : Notifier
    {
        // 파티션 이름
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

        public Scan(string name)
        {
            this.name = name;
        }
    }
}
