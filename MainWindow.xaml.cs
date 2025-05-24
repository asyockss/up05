using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using inventory.Pages;

namespace inventory
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateToLoginPage();
        }

        public void NavigateToLoginPage() => MainFrame.Navigate(new LoginPage());
        public void NavigateToMainPage() => MainFrame.Navigate(new MainPage());
        public void NavigateToPage(Page page) => MainFrame.Navigate(page);

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void ShowEquipment_Click(object sender, RoutedEventArgs e) => NavigateToPage(new EquipmentPage());
        private void AddEquipment_Click(object sender, RoutedEventArgs e) => NavigateToPage(new AddEditEquipmentPage());
        private void ShowConsumables_Click(object sender, RoutedEventArgs e) => NavigateToPage(new ConsumablesPage());
        private void AddConsumable_Click(object sender, RoutedEventArgs e) => NavigateToPage(new AddEditConsumablePage());
        private void ShowRooms_Click(object sender, RoutedEventArgs e) => NavigateToPage(new RoomsPage());
        private void AddRoom_Click(object sender, RoutedEventArgs e) => NavigateToPage(new AddEditRoomPage());
        private void ShowInventory_Click(object sender, RoutedEventArgs e) => NavigateToPage(new InventoryPage());
        private void AddInventory_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Только администраторы могут управлять инвентаризациями");
                return;
            }
            NavigateToPage(new AddEditInventoryPage());
        }
        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Только администраторы могут управлять пользователями");
                return;
            }
            NavigateToPage(new UsersPage());
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Только администраторы могут управлять пользователями");
                return;
            }
            NavigateToPage(new UsersPage());
        }
        private void ShowSoftware_Click(object sender, RoutedEventArgs e) => NavigateToPage(new SoftwarePage());
        private void AddSoftware_Click(object sender, RoutedEventArgs e) => NavigateToPage(new AddEditSoftwarePage());
    }
}