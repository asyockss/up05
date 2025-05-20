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
    public partial class AddEditRoomPage : Page, INotifyPropertyChanged
    {
        public Room CurrentRoom { get; set; }
        public string Title => CurrentRoom.Id == 0 ? "Добавить аудиторию" : "Редактировать аудиторию";
        public List<User> Users { get; set; }
        public bool IsMenuVisible => true;

        public AddEditRoomPage(Room room = null)
        {
            InitializeComponent();
            CurrentRoom = room ?? new Room();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            Users = UserContext.AllUsers().Cast<User>().ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentRoom.Name) || string.IsNullOrEmpty(CurrentRoom.ShortName))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }
            RoomContext.Save(CurrentRoom, CurrentRoom.Id != 0);
            NavigateBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();

        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new RoomsPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}