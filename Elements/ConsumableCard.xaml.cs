using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using inventory.Context.MySql;
using inventory.Models;
using inventory.Pages;

namespace inventory.Elements
{
    public partial class ConsumableCard : UserControl
    {
        public ConsumableCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Consumable consumable)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditConsumablePage(consumable));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Consumable consumable &&
                MessageBox.Show("Вы уверены, что хотите удалить этот расходник?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ConsumableContext.Delete(consumable.Id);
                RefreshParentPage();
            }
        }

        private void RefreshParentPage()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is ConsumablesPage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is ConsumablesPage page) page.LoadData();
        }
    }
}