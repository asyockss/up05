using System.ComponentModel;

namespace inventory.Models
{
    public class Equipment : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private byte[] _photo;
        private string _inventoryNumber;
        private int? _roomId;
        private int? _responsibleId;
        private int? _tempResponsibleId;
        private decimal _cost;
        private string _comment;
        private int? _statusId;
        private int? _modelId;
        private int? _equipmentTypeId;
        private int? _directionId;

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
        public byte[] Photo
        {
            get => _photo;
            set { _photo = value; OnPropertyChanged(nameof(Photo)); }
        }
        public string InventoryNumber
        {
            get => _inventoryNumber;
            set { _inventoryNumber = value; OnPropertyChanged(nameof(InventoryNumber)); }
        }
        public int? RoomId
        {
            get => _roomId;
            set { _roomId = value; OnPropertyChanged(nameof(RoomId)); }
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
        public decimal Cost
        {
            get => _cost;
            set { _cost = value; OnPropertyChanged(nameof(Cost)); }
        }
        public string Comment
        {
            get => _comment;
            set { _comment = value; OnPropertyChanged(nameof(Comment)); }
        }
        public int? StatusId
        {
            get => _statusId;
            set { _statusId = value; OnPropertyChanged(nameof(StatusId)); }
        }
        public int? ModelId
        {
            get => _modelId;
            set { _modelId = value; OnPropertyChanged(nameof(ModelId)); }
        }
        public int? EquipmentTypeId
        {
            get => _equipmentTypeId;
            set { _equipmentTypeId = value; OnPropertyChanged(nameof(EquipmentTypeId)); }
        }
        public int? DirectionId
        {
            get => _directionId;
            set { _directionId = value; OnPropertyChanged(nameof(DirectionId)); }
        }

        // Связанные объекты (для отображения в UI)
        public EquipmentType EquipmentType { get; set; }
        public Status Status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}