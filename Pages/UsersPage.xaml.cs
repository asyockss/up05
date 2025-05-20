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
    public partial class UsersPage : Page, INotifyPropertyChanged
    {
        private List<User> _userList;
        private string _searchText;
        private string _selectedRoleFilter;

        public List<User> UserList
        {
            get => _userList;
            set { _userList = value; OnPropertyChanged(nameof(UserList)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterUsers(); }
        }

        public List<string> RoleFilters => new List<string> { "Все", "Admin", "User" };

        public string SelectedRoleFilter
        {
            get => _selectedRoleFilter;
            set { _selectedRoleFilter = value; OnPropertyChanged(nameof(SelectedRoleFilter)); FilterUsers(); }
        }

        public bool IsMenuVisible => true;

        public UsersPage()
        {
            InitializeComponent();
            DataContext = this;
            SelectedRoleFilter = "Все";
            LoadUsers();
        }

        private void LoadUsers()
        {
            UserList = UserContext.AllUsers().Cast<User>().ToList();
        }

        private void FilterUsers()
        {
            var users = UserContext.AllUsers().Cast<User>();
            if (!string.IsNullOrEmpty(SearchText))
                users = users.Where(u => u.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                         u.Login.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            if (SelectedRoleFilter != "Все")
                users = users.Where(u => u.Role.Equals(SelectedRoleFilter, StringComparison.OrdinalIgnoreCase));
            UserList = users.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditUserPage());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadUsers();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}