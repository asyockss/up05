using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using inventory.Context.MySql;
using inventory.Models;
using inventory.Pages;

namespace inventory.Elements
{
    public partial class RoomCard : UserControl
    {
        public RoomCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Room room)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditRoomPage(room));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Room room &&
                MessageBox.Show("Вы уверены, что хотите удалить эту аудиторию?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                RoomContext.Delete(room.Id);
                RefreshParentPage();
            }
        }

        private void RefreshParentPage()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is RoomsPage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is RoomsPage page) page.LoadRooms();
        }
    }
}