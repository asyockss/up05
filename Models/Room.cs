using System.ComponentModel;

namespace inventory.Models
{
    public class Room : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _shortName;
        private int? _responsibleId;
        private int? _tempResponsibleId;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string ShortName
        {
            get => _shortName;
            set { _shortName = value; OnPropertyChanged(nameof(ShortName)); }
        }
        public int? ResponsibleId
        {
            get => _responsibleId;
            set { _responsibleId = value; OnPropertyChanged(nameof(ResponsibleId)); }
        }
        public int? TempResponsibleId
        {
            get => _tempResponsibleId;
            set { _tempResponsibleId = value; OnPropertyChanged(nameof(TempResponsibleId)); }
        }

        public User ResponsibleUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}