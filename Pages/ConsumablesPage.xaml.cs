using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;
using inventory.Models.inventory.Models;

namespace inventory.Pages
{
    public partial class ConsumablesPage : Page, INotifyPropertyChanged
    {
        private List<Consumable> _consumableList;
        private List<ConsumableType> _consumableTypes;
        private int? _selectedTypeFilter;

        public List<Consumable> ConsumableList
        {
            get => _consumableList;
            set { _consumableList = value; OnPropertyChanged(nameof(ConsumableList)); }
        }

        public List<ConsumableType> ConsumableTypes
        {
            get => _consumableTypes;
            set { _consumableTypes = value; OnPropertyChanged(nameof(ConsumableTypes)); }
        }

        public int? SelectedTypeFilter
        {
            get => _selectedTypeFilter;
            set { _selectedTypeFilter = value; OnPropertyChanged(nameof(SelectedTypeFilter)); FilterConsumables(); }
        }

        public bool IsMenuVisible => true;

        public ConsumablesPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        public void LoadData()
        {
            var consumables = new ConsumableContext().AllConsumables();
            var types = new ConsumableTypeContext().AllConsumableTypes().ToDictionary(t => t.Id, t => t);
            foreach (var item in consumables)
            {
                if (item.ConsumableTypeId.HasValue && types.ContainsKey(item.ConsumableTypeId.Value))
                    item.ConsumableType = types[item.ConsumableTypeId.Value];
            }
            ConsumableList = consumables;
            ConsumableTypes = new ConsumableTypeContext().AllConsumableTypes();
        }

        private void FilterConsumables()
        {
            ConsumableList = new ConsumableContext().AllConsumables()
                .Where(c => !SelectedTypeFilter.HasValue || c.ConsumableTypeId == SelectedTypeFilter)
                .ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditConsumablePage());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadData();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}