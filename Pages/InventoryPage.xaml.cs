using System.Windows;
using System.Windows.Controls;
using inventory.ViewModels;

namespace inventory.Pages
{
    public partial class InventoryPage : Page
    {
        public InventoryPage()
        {
            InitializeComponent();
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Доступ запрещен. Требуются права администратора.");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToMainPage();
                return;
            }
            DataContext = new InventoryViewModel();
            DataContext = new MainPageViewModel();
        }

        public bool IsMenuVisible => true;
    }
}
