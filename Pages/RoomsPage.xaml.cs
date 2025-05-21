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
    public partial class RoomsPage : Page, INotifyPropertyChanged
    {
        private RoomContext roomContext;
        private List<Room> _roomList;
        private string _searchText;

        public List<Room> RoomList
        {
            get => _roomList;
            set { _roomList = value; OnPropertyChanged(nameof(RoomList)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterRooms(); }
        }

        public bool IsMenuVisible => true;

        public RoomsPage()
        {
            InitializeComponent();
            DataContext = this;
            roomContext = new RoomContext();
            LoadRooms();
            
        }

        public void LoadRooms()
        {
            RoomList = roomContext.AllRooms().Cast<Room>().ToList();
        }

        private void FilterRooms()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                LoadRooms();
            }
            else
            {
                RoomList = roomContext.AllRooms()
                    .Cast<Room>()
                    .Where(r => r.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               r.ShortName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditRoomPage());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadRooms();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}