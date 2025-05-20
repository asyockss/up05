using System;
using System.ComponentModel;

namespace inventory.Models
{
    public class Consumable : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _description;
        private DateTime _receiptDate;
        private byte[] _image;
        private int _quantity;
        private int? _responsibleId;
        private int? _tempResponsibleId;
        private int? _consumableTypeId;

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
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
        public DateTime ReceiptDate
        {
            get => _receiptDate;
            set { _receiptDate = value; OnPropertyChanged(nameof(ReceiptDate)); }
        }
        public byte[] Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
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
        public int? ConsumableTypeId
        {
            get => _consumableTypeId;
            set { _consumableTypeId = value; OnPropertyChanged(nameof(ConsumableTypeId)); }
        }
        public ConsumableType ConsumableType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}