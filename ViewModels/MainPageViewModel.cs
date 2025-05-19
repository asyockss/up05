using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inventory.Pages;
using System.Windows.Input;
using System.Windows;

namespace inventory.ViewModels
{
    public class MainPageViewModel
    {
        public ICommand NavigateToEquipmentCommand { get; }
        public ICommand NavigateToConsumablesCommand { get; }
        public ICommand NavigateToRoomsCommand { get; }
        public ICommand NavigateToInventoryCommand { get; }
        public ICommand NavigateToUsersCommand { get; }

        public bool IsAdmin => CurrentUser.IsAdmin;

        public MainPageViewModel()
        {
            NavigateToEquipmentCommand = new RelayCommand(NavigateToEquipment);
            NavigateToConsumablesCommand = new RelayCommand(NavigateToConsumables);
            NavigateToRoomsCommand = new RelayCommand(NavigateToRooms);
            NavigateToInventoryCommand = new RelayCommand(NavigateToInventory);
            NavigateToUsersCommand = new RelayCommand(NavigateToUsers);

            ExitCommand = new RelayCommand(Exit);
        }

        private void NavigateToEquipment()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new EquipmentPage());
        }

        private void NavigateToConsumables()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new ConsumablesPage());
        }

        private void NavigateToRooms()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new RoomsPage());
        }

        private void NavigateToInventory()
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Только администраторы могут запускать инвентаризацию");
                return;
            }
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new InventoryPage());
        }

        private void NavigateToUsers()
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Только администраторы могут управлять пользователями");
                return;
            }
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new UsersPage());
        }
        public ICommand ExitCommand { get; }


        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}