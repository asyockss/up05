using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace inventory.Models
{
    public class Software : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _version;
        private int? _developerId;
        private int? _equipmentId;
        private Developer _developer;
        private Equipment _equipment;

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

        public string Version
        {
            get => _version;
            set { _version = value; OnPropertyChanged(nameof(Version)); }
        }

        public int? DeveloperId
        {
            get => _developerId;
            set { _developerId = value; OnPropertyChanged(nameof(DeveloperId)); }
        }

        public int? EquipmentId
        {
            get => _equipmentId;
            set { _equipmentId = value; OnPropertyChanged(nameof(EquipmentId)); }
        }

        public Developer Developer
        {
            get => _developer;
            set { _developer = value; OnPropertyChanged(nameof(Developer)); }
        }

        public Equipment Equipment
        {
            get => _equipment;
            set { _equipment = value; OnPropertyChanged(nameof(Equipment)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}