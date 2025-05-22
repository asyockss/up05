using System.ComponentModel;

namespace inventory.Models
{
    namespace inventory.Models
    {
        public class ConsumableType : INotifyPropertyChanged
        {
            private int _id;
            private string _type;

            public int Id
            {
                get => _id;
                set { _id = value; OnPropertyChanged(nameof(Id)); }
            }
            public string Type
            {
                get => _type;
                set { _type = value; OnPropertyChanged(nameof(Type)); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}