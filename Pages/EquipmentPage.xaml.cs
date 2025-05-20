using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;

namespace inventory.Pages
{
    public partial class EquipmentPage : Page, INotifyPropertyChanged
    {
        private List<Equipment> _equipmentList;
        private string _searchText;

        public List<Equipment> EquipmentList
        {
            get => _equipmentList;
            set { _equipmentList = value; OnPropertyChanged(nameof(EquipmentList)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterEquipment(); }
        }

        public bool IsMenuVisible => true;

        public EquipmentPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadEquipment();
        }

        private void LoadEquipment()
        {
            var equipment = new EquipmentContext().AllEquipment();
            var types = new EquipmentTypeContext().AllEquipmentTypes().ToDictionary(t => t.Id, t => t);
            var statuses = new StatusContext().AllStatuses().ToDictionary(s => s.Id, s => s);

            foreach (var item in equipment)
            {
                if (item.EquipmentTypeId.HasValue && types.ContainsKey(item.EquipmentTypeId.Value))
                    item.EquipmentType = types[item.EquipmentTypeId.Value];
                if (item.StatusId.HasValue && statuses.ContainsKey(item.StatusId.Value))
                    item.Status = statuses[item.StatusId.Value];
            }
            EquipmentList = equipment;
        }

        private void FilterEquipment()
        {
            if (string.IsNullOrEmpty(SearchText))
                LoadEquipment();
            else
                EquipmentList = EquipmentList.Where(e => e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                                         e.InventoryNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                                             .ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditEquipmentPage());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadEquipment();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}