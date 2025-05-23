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
    public partial class AddEditInventoryPage : Page, INotifyPropertyChanged
    {
        private Inventory _currentInventory;
        public Inventory CurrentInventory
        {
            get => _currentInventory;
            set { _currentInventory = value; OnPropertyChanged(nameof(CurrentInventory)); }
        }
        public new string Title => CurrentInventory.Id == 0 ? "Добавить инвентаризацию" : "Редактировать инвентаризацию";
        public List<User> Users { get; set; }
        public bool IsMenuVisible => true;

        public AddEditInventoryPage(Inventory inventory = null)
        {
            InitializeComponent();
            CurrentInventory = inventory ?? new Inventory
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            Users = new UserContext().AllUsers();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentInventory.Name))
            {
                MessageBox.Show("Заполните обязательное поле: Название");
                return;
            }
            if (CurrentInventory.StartDate == default || CurrentInventory.EndDate == default)
            {
                MessageBox.Show("Заполните обязательные поля: Дата начала, Дата окончания");
                return;
            }
            try
            {
                new InventoryContext().Save(CurrentInventory, CurrentInventory.Id != 0);
                NavigateBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();

        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new InventoryPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}