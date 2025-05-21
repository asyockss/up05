using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;

namespace inventory.Pages
{
    public partial class AddEditUserPage : Page, INotifyPropertyChanged
    {
        private UserContext userContext;
        public User CurrentUser { get; set; }
        public new string Title => CurrentUser.Id == 0 ? "Добавить пользователя" : "Редактировать пользователя";
        public List<string> Roles => new List<string> { "admin", "teacher", "employee" };
        public bool IsMenuVisible => true;

        public AddEditUserPage(User user = null)
        {
            InitializeComponent();
            CurrentUser = user ?? new User();
            DataContext = this;
            userContext = new UserContext();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentUser.Login) || string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(CurrentUser.LastName) || string.IsNullOrEmpty(CurrentUser.FirstName) ||
                string.IsNullOrEmpty(CurrentUser.Role))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }
            CurrentUser.Password = PasswordBox.Password;
            userContext.Save(CurrentUser, CurrentUser.Id != 0);
            NavigateBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();

        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new UsersPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
