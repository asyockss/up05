using System.Windows;
using System.Windows.Controls;
using inventory.Pages;

namespace inventory.Pages
{
    public partial class MainPage : Page
    {
        public bool IsMenuVisible => false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void NavigateToEquipment_Click(object sender, RoutedEventArgs e) => NavigateTo(new EquipmentPage());
        private void NavigateToConsumables_Click(object sender, RoutedEventArgs e) => NavigateTo(new ConsumablesPage());
        private void NavigateToRooms_Click(object sender, RoutedEventArgs e) => NavigateTo(new RoomsPage());
        private void NavigateToInventory_Click(object sender, RoutedEventArgs e) => NavigateTo(new InventoryPage());
        private void NavigateToUsers_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Только администраторы могут управлять пользователями");
                return;
            }
            NavigateTo(new UsersPage());
        }

        private void NavigateTo(Page page)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(page);
        }
    }
}