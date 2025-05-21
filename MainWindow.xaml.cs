using System.Windows;
using System.Windows.Controls;
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
        private void ShowUsers_Click(object sender, RoutedEventArgs e) => NavigateToPage(new UsersPage());
    }
}