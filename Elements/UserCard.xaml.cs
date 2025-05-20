using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using inventory.Context.MySql;
using inventory.Models;
using inventory.Pages;

namespace inventory.Elements
{
    public partial class UserCard : UserControl
    {
        private UserContext userContext;
        public UserCard()
        {
            InitializeComponent();
            userContext = new UserContext();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is User user)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditUserPage(user));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is User user &&
                MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                userContext.Delete(user.Id);
                RefreshParentPage();
            }
        }

        private void RefreshParentPage()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is UsersPage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is UsersPage page) page.LoadUsers();
        }
    }
}