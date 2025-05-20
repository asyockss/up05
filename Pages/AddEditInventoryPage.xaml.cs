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
        private InventoryContext inventoryContext;
        private UserContext userContext;

        public Inventory CurrentInventory { get; set; }
        public new string Title => CurrentInventory.Id == 0 ? "Добавить инвентаризацию" : "Редактировать инвентаризацию";
        public List<User> Users { get; set; }
        public DateTime MinDate { get; } = DateTime.Now;
        public DateTime MaxDate { get; } = DateTime.Now.AddYears(1);
        public bool IsMenuVisible => true;

        public AddEditInventoryPage(Inventory inventory = null)
        {
            InitializeComponent();
            CurrentInventory = inventory ?? new Inventory();
            inventoryContext = new InventoryContext();
            userContext = new UserContext();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            Users = userContext.AllUsers().Cast<User>().ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentInventory.Name) || CurrentInventory.StartDate == default || CurrentInventory.EndDate == default)
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            if (CurrentInventory.StartDate > CurrentInventory.EndDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания");
                return;
            }

            // Убедитесь, что UserId установлен и существует в базе данных
            if (CurrentInventory.UserId == 0)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }

            // Проверяем, существует ли пользователь с таким UserId
            var userExists = Users.Any(user => user.Id == CurrentInventory.UserId);
            if (!userExists)
            {
                MessageBox.Show("Выбранный пользователь не существует в базе данных");
                return;
            }

            // Сохраняем инвентаризацию
            inventoryContext.Save(CurrentInventory, CurrentInventory.Id != 0);
            NavigateBack();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();

        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new InventoryPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
