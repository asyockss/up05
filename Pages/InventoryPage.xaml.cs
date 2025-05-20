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
    public partial class InventoryPage : Page, INotifyPropertyChanged
    {
        private InventoryContext inventoryContext;
        private List<Inventory> _inventoryList;
        private DateTime? _startDateFilter;
        private DateTime? _endDateFilter;

        public List<Inventory> InventoryList
        {
            get => _inventoryList;
            set { _inventoryList = value; OnPropertyChanged(nameof(InventoryList)); }
        }

        public DateTime? StartDateFilter
        {
            get => _startDateFilter;
            set { _startDateFilter = value; OnPropertyChanged(nameof(StartDateFilter)); FilterInventories(); }
        }

        public DateTime? EndDateFilter
        {
            get => _endDateFilter;
            set { _endDateFilter = value; OnPropertyChanged(nameof(EndDateFilter)); FilterInventories(); }
        }

        public bool IsMenuVisible => true;

        public InventoryPage()
        {
            InitializeComponent();
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Доступ запрещен. Требуются права администратора.");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToMainPage();
                return;
            }
            DataContext = this;
            LoadInventories();
        }

        public void LoadInventories()
        {
            InventoryList = inventoryContext.AllInventorys().Cast<Inventory>().ToList();
        }

        private void FilterInventories()
        {
            var inventories = inventoryContext.AllInventorys().Cast<Inventory>();
            if (StartDateFilter.HasValue)
                inventories = inventories.Where(i => i.StartDate >= StartDateFilter.Value);
            if (EndDateFilter.HasValue)
                inventories = inventories.Where(i => i.EndDate <= EndDateFilter.Value);
            InventoryList = inventories.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавление новой инвентаризации еще не реализовано.");
        }

        private void PerformCheck_Click(object sender, RoutedEventArgs e)
        {
            // Implement check logic if needed
            MessageBox.Show("Проведение проверки еще не реализовано.");
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            // Implement report logic if needed
            MessageBox.Show("Генерация отчета еще не реализована.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}